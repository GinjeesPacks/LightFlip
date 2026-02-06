using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace LightFlip
{
    internal static class StartupManager
    {
        private const string RunKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string ValueName = "LightFlip";

        public static string InstallDir =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LightFlip");

        public static string InstalledExePath => Path.Combine(InstallDir, "LightFlip.exe");

        public static void Apply(bool enabled)
        {
            try
            {
                using var key = Registry.CurrentUser.CreateSubKey(RunKeyPath, writable: true);
                if (key == null) return;

                if (enabled)
                {
                    Directory.CreateDirectory(InstallDir);

                    string currentExe = Application.ExecutablePath;
                    string targetExe = InstalledExePath;

                   
                    if (!string.Equals(currentExe, targetExe, StringComparison.OrdinalIgnoreCase))
                    {
                        File.Copy(currentExe, targetExe, overwrite: true);
                    }

                    key.SetValue(ValueName, $"\"{targetExe}\"");
                }
                else
                {
                    if (key.GetValue(ValueName) != null)
                        key.DeleteValue(ValueName, throwOnMissingValue: false);
                }
            }
            catch
            {
             
            }
        }

      
        public static void RemoveInstalledExe()
        {
            try
            {
                if (File.Exists(InstalledExePath))
                    File.Delete(InstalledExePath);
            }
            catch { }
        }
    }
}
