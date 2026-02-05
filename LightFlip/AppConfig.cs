using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace LightFlip
{
    public sealed class AppConfig
    {
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
    }

    public sealed class ColorProfile
    {
        public float Brightness { get; set; } = 50f; 
        public float Contrast { get; set; } = 50f;   
        public float Gamma { get; set; } = 1.00f;    

        public static ColorProfile Neutral()
        {
            return new ColorProfile { Brightness = 50f, Contrast = 50f, Gamma = 1.00f };
        }
    }
}
