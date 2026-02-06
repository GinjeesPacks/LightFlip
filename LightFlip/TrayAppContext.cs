using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;

namespace LightFlip
{
    internal sealed class TrayAppContext : ApplicationContext
    {
        private NotifyIcon? tray;
        private HotkeyHostForm? host;
        private Form1? settingsForm;

        private AppConfig config;

        private System.Windows.Forms.Timer pollTimer;

        private uint activePid;
        private string activeExePath = string.Empty;
        private string activeProcessName = string.Empty;
        private GameProfile? activeProfile;

        private uint trackedGamePid;
        private GameProfile? trackedGameProfile;

        
        private uint registeredMods = 0;
        private uint registeredKey = 0;

        
        private bool trackedGameHadFocus = false;

        public TrayAppContext()
        {
            config = AppConfig.Load();

            
            GammaRamp.CaptureBaselineIfNeeded();

            StartupManager.Apply(config.StartWithWindows);

            host = new HotkeyHostForm();
            host.HotkeyPressed += (s, e) => ToggleForActiveGame();

            tray = new NotifyIcon
            {
                Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath) ?? SystemIcons.Application,
                Visible = true,
                Text = "LightFlip"
            };

            var menu = new ContextMenuStrip();
            menu.Items.Add("Toggle", null, (s, e) => ToggleForActiveGame());
            menu.Items.Add("Settings...", null, (s, e) => ShowSettings());
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add("Exit", null, (s, e) => ExitApp());
            tray.ContextMenuStrip = menu;

            tray.DoubleClick += (s, e) => ShowSettings();

            
            EnsureHotkeyRegistered(null);

            pollTimer = new System.Windows.Forms.Timer();
            pollTimer.Interval = 500;
            pollTimer.Tick += (s, e) => PollForegroundGame();
            pollTimer.Start();

            PollForegroundGame();

            if (!(config.StartMinimized && config.MinimizeToTray))
                ShowSettings();

            UpdateTrayText();
        }

        
        private void EnsureHotkeyRegistered(GameProfile? profile)
        {
            uint mods = config.HotkeyModifiers;
            uint key = config.HotkeyKey;

            if (profile != null && profile.HotkeyKey != 0)
            {
                mods = profile.HotkeyModifiers;
                key = profile.HotkeyKey;
            }

            if (mods == registeredMods && key == registeredKey)
                return;

            host?.RegisterHotkey(mods, key, showError: false);
            registeredMods = mods;
            registeredKey = key;
        }

        private static void TryApplyStartupSetting(bool enabled)
        {
            const string runKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
            const string valueName = "LightFlip";

            try
            {
                using var key = Registry.CurrentUser.CreateSubKey(runKeyPath, writable: true);
                if (key == null) return;

                if (enabled)
                {
                    string appDir = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "LightFlip");

                    Directory.CreateDirectory(appDir);

                    string sourceDir = AppContext.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar);
                    string targetExe = Path.Combine(appDir, "LightFlip.exe");

                    if (!string.Equals(sourceDir, appDir, StringComparison.OrdinalIgnoreCase))
                    {
                        CopyDirectory(sourceDir, appDir);
                    }

                    key.SetValue(valueName, $"\"{targetExe}\"");
                }
                else
                {
                    if (key.GetValue(valueName) != null)
                        key.DeleteValue(valueName, throwOnMissingValue: false);
                }
            }
            catch
            {
            }
        }

        private static void CopyDirectory(string sourceDir, string destDir)
        {
            foreach (string dir in Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories))
            {
                string rel = Path.GetRelativePath(sourceDir, dir);
                string dst = Path.Combine(destDir, rel);
                Directory.CreateDirectory(dst);
            }

            foreach (string file in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                string rel = Path.GetRelativePath(sourceDir, file);
                string dst = Path.Combine(destDir, rel);
                Directory.CreateDirectory(Path.GetDirectoryName(dst)!);
                File.Copy(file, dst, true);
            }
        }

        private static bool ProcessByIdExists(uint pid)
        {
            if (pid == 0) return false;

            try
            {
                using (var p = Process.GetProcessById((int)pid))
                {
                    return !p.HasExited;
                }
            }
            catch
            {
                return false;
            }
        }

        private void PollForegroundGame()
        {
            
            if (trackedGameProfile != null && trackedGamePid != 0)
            {
                if (!ProcessByIdExists(trackedGamePid))
                {
                    var exitedProfile = trackedGameProfile;

                    trackedGamePid = 0;
                    trackedGameProfile = null;
                    trackedGameHadFocus = false;

                    activePid = 0;
                    activeExePath = string.Empty;
                    activeProcessName = string.Empty;
                    activeProfile = null;

                    if (exitedProfile != null && exitedProfile.RevertOnClose)
                    {
                        try { GammaRamp.ApplyToNeutralAll(); } catch { }
                    }

                    
                    EnsureHotkeyRegistered(null);

                    UpdateTrayText();
                    return;
                }
            }

            IntPtr hwnd = NativeMethods.GetForegroundRootWindowSafe();
            uint pid = NativeMethods.GetForegroundRootProcessId();
            if (pid == 0 || hwnd == IntPtr.Zero)
                return;

            try
            {
                if (Process.GetCurrentProcess().Id == (int)pid)
                    return;
            }
            catch { }

            string? exe = NativeMethods.TryGetProcessImagePath(pid);
            string? procName = NativeMethods.TryGetProcessName(pid);

            string exeSafe = exe ?? string.Empty;
            string procNameSafe = procName ?? string.Empty;

            if (pid == activePid &&
                string.Equals(exeSafe, activeExePath, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(procNameSafe, activeProcessName, StringComparison.OrdinalIgnoreCase))
                return;

            activePid = pid;
            activeExePath = exeSafe;
            activeProcessName = procNameSafe;

            var found = FindProfileByExeOrProcessName(exeSafe, procNameSafe);

            
            if (trackedGameProfile != null && trackedGamePid != 0)
            {
                bool nowFocusedIsTracked = (pid == trackedGamePid);

                
                if (!nowFocusedIsTracked)
                {
                    if (trackedGameProfile.RevertOnClose)
                    {
                        try { GammaRamp.ApplyToNeutralAll(); } catch { }
                    }
                }

                trackedGameHadFocus = nowFocusedIsTracked;
            }

            if (found != null)
            {
                activeProfile = found;
                trackedGamePid = pid;
                trackedGameProfile = found;
                trackedGameHadFocus = true;

                
                EnsureHotkeyRegistered(activeProfile);

                try
                {
                    var p = activeProfile.LastBright ? activeProfile.Bright : activeProfile.Normal;
                    GammaRamp.ApplyForWindow(hwnd, p);
                }
                catch { }
            }
            else
            {
                activeProfile = null;

                
                EnsureHotkeyRegistered(null);
            }

            UpdateTrayText();
        }

        private GameProfile? FindProfileByExe(string exePath)
        {
            if (string.IsNullOrWhiteSpace(exePath))
                return null;

            string normExe = NormalizeExePath(exePath);

            return config.Games.FirstOrDefault(g =>
                !string.IsNullOrWhiteSpace(g.ExePath) &&
                string.Equals(NormalizeExePath(g.ExePath), normExe, StringComparison.OrdinalIgnoreCase));
        }

        private GameProfile? FindProfileByExeOrProcessName(string? exePath, string? processName)
        {
            
            if (!string.IsNullOrWhiteSpace(exePath))
            {
                var byExe = FindProfileByExe(exePath);
                if (byExe != null)
                    return byExe;
            }

            
            if (!string.IsNullOrWhiteSpace(processName))
            {
                string pn = processName.Trim().Trim('"');
                try { pn = Path.GetFileNameWithoutExtension(pn); } catch { }

                if (!string.IsNullOrWhiteSpace(pn))
                {
                    return config.Games.FirstOrDefault(g =>
                        !string.IsNullOrWhiteSpace(g.ExePath) &&
                        string.Equals(Path.GetFileNameWithoutExtension(g.ExePath), pn, StringComparison.OrdinalIgnoreCase));
                }
            }

            return null;
        }

private static string NormalizeExePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return string.Empty;

            try
            {
                return Path.GetFullPath(path.Trim().Trim('"'));
            }
            catch
            {
                return path.Trim().Trim('"');
            }
        }

        private void ToggleForActiveGame()
        {
            if (activeProfile == null)
            {
                return;
            }

            trackedGameProfile = activeProfile;
            trackedGamePid = activePid;
            trackedGameHadFocus = true;

            activeProfile.LastBright = !activeProfile.LastBright;

            try { config.Save(); } catch { }

            IntPtr hwnd = NativeMethods.GetForegroundWindowSafe();
            try
            {
                var p = activeProfile.LastBright ? activeProfile.Bright : activeProfile.Normal;
                GammaRamp.ApplyForWindow(hwnd, p);
            }
            catch { }

            if (tray != null)
            {
                tray.BalloonTipTitle = "LightFlip";
                tray.BalloonTipText = activeProfile.LastBright ? "Bright profile ON" : "Bright profile OFF";
                tray.ShowBalloonTip(700);
            }

            UpdateTrayText();
        }

        private void ShowSettings()
        {
            if (settingsForm != null && !settingsForm.IsDisposed)
            {
                settingsForm.Show();
                settingsForm.WindowState = FormWindowState.Normal;
                settingsForm.BringToFront();
                return;
            }

            settingsForm = new Form1(config);
            settingsForm.ConfigApplied += (s, cfg) =>
            {
                config = cfg;

                TryApplyStartupSetting(config.StartWithWindows);

                
                activePid = 0;
                activeExePath = string.Empty;
                activeProcessName = string.Empty;
                activeProfile = null;
                trackedGamePid = 0;
                trackedGameProfile = null;
                trackedGameHadFocus = false;

                
                PollForegroundGame();

                
                EnsureHotkeyRegistered(activeProfile);

                UpdateTrayText();
            };

            settingsForm.FormClosing += (s, e) =>
            {
                if (config.MinimizeToTray && !settingsForm!.AllowHardClose)
                {
                    e.Cancel = true;
                    settingsForm.Hide();
                }
            };

            settingsForm.Show();
        }

        private void UpdateTrayText()
        {
            if (tray == null) return;

            
            uint mods = config.HotkeyModifiers;
            uint key = config.HotkeyKey;

            if (activeProfile != null && activeProfile.HotkeyKey != 0)
            {
                mods = activeProfile.HotkeyModifiers;
                key = activeProfile.HotkeyKey;
            }

            string hk = HotkeyText.Format(mods, key);

            if (activeProfile != null)
            {
                string mode = activeProfile.LastBright ? "BRIGHT" : "NORMAL";

                string name = "";
                try
                {
                    name = !string.IsNullOrWhiteSpace(activeProfile.DisplayName)
                        ? activeProfile.DisplayName
                        : Path.GetFileNameWithoutExtension(activeProfile.ExePath ?? "");
                }
                catch { name = ""; }

                if (string.IsNullOrWhiteSpace(name))
                    name = "(game)";

                string text = $"LightFlip: {name} {mode} ({hk})";
                tray.Text = text.Length > 63 ? text.Substring(0, 63) : text;
            }
            else
            {
                string text = $"LightFlip ({hk})";
                tray.Text = text.Length > 63 ? text.Substring(0, 63) : text;
            }
        }

        private void ExitApp()
        {
            try { GammaRamp.ApplyToNeutralAll(); } catch { }

            try
            {
                pollTimer.Stop();
                pollTimer.Dispose();
            }
            catch { }

            try
            {
                if (tray != null)
                {
                    tray.Visible = false;
                    tray.Dispose();
                    tray = null;
                }
            }
            catch { }

            try
            {
                if (host != null)
                {
                    host.SafeClose();
                    host = null;
                }
            }
            catch { }

            try
            {
                if (settingsForm != null)
                {
                    settingsForm.AllowHardClose = true;
                    settingsForm.Close();
                    settingsForm.Dispose();
                    settingsForm = null;
                }
            }
            catch { }

            ExitThread();
        }
    }
}
