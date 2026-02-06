using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace LightFlip
{
    public sealed class AppConfig
    {
        // GLOBAL hotkey (fallback when a game does not define its own hotkey)
        public uint HotkeyModifiers { get; set; } = HotkeyMods.MOD_CONTROL | HotkeyMods.MOD_ALT;
        public uint HotkeyKey { get; set; } = (uint)Keys.G;

        public bool MinimizeToTray { get; set; } = true;
        public bool StartWithWindows { get; set; } = false;
        public bool StartMinimized { get; set; } = false;

        public string LastSelectedGameExePath { get; set; } = string.Empty;

        public List<GameProfile> Games { get; set; } = new List<GameProfile>();

        public static string ConfigPath
        {
            get
            {
                string dir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "LightFlip");
                Directory.CreateDirectory(dir);
                return Path.Combine(dir, "config.json");
            }
        }

        public void Save()
        {
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ConfigPath, json);
        }

        public static AppConfig Load()
        {
            try
            {
                if (File.Exists(ConfigPath))
                {
                    var json = File.ReadAllText(ConfigPath);
                    var cfg = JsonSerializer.Deserialize<AppConfig>(json);
                    if (cfg != null)
                    {
                        cfg.Games ??= new List<GameProfile>();
                        cfg.LastSelectedGameExePath ??= string.Empty;

                        // Safety: ensure nested objects exist (older configs)
                        foreach (var g in cfg.Games)
                        {
                            g.ExePath ??= string.Empty;
                            g.DisplayName ??= string.Empty;
                            g.Normal ??= ColorProfile.Neutral();
                            g.Bright ??= ColorProfile.Neutral();
                        }

                        return cfg;
                    }
                }
            }
            catch { }

            return new AppConfig();
        }
    }

    public sealed class GameProfile
    {
        public string ExePath { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;

        public bool RevertOnClose { get; set; } = true;

        public bool LastBright { get; set; } = false;

        public ColorProfile Normal { get; set; } = ColorProfile.Neutral();
        public ColorProfile Bright { get; set; } = ColorProfile.Neutral();

        // NEW: per-game hotkey (0 = use global hotkey)
        public uint HotkeyModifiers { get; set; } = 0;
        public uint HotkeyKey { get; set; } = 0;
    }

    public sealed class ColorProfile
    {
        public float Brightness { get; set; } = 50f;
        public float Contrast { get; set; } = 50f;
        public float Gamma { get; set; } = 1.00f;

        // NEW: -100..+100 (Cool -> Warm)
        public float Temperature { get; set; } = 0f;

        public static ColorProfile Neutral()
        {
            return new ColorProfile
            {
                Brightness = 50f,
                Contrast = 50f,
                Gamma = 1.00f,
                Temperature = 0f
            };
        }
    }
}
