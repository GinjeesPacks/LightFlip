using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LightFlip
{
    internal static class HotkeyMods
    {
        public const uint MOD_ALT = 0x0001;
        public const uint MOD_CONTROL = 0x0002;
        public const uint MOD_SHIFT = 0x0004;
        public const uint MOD_WIN = 0x0008;

        
        public const uint MOD_NOREPEAT = 0x4000;
    }

    internal static class HotkeyText
    {
        public static string Format(uint mods, uint key)
        {
            if (key == 0) return "None";

            string s = "";
            if ((mods & HotkeyMods.MOD_CONTROL) != 0) s += "Ctrl + ";
            if ((mods & HotkeyMods.MOD_ALT) != 0) s += "Alt + ";
            if ((mods & HotkeyMods.MOD_SHIFT) != 0) s += "Shift + ";
            if ((mods & HotkeyMods.MOD_WIN) != 0) s += "Win + ";
            s += ((Keys)key).ToString();
            return s;
        }
    }

    internal sealed class HotkeyHostForm : Form
    {
        private const int WM_HOTKEY = 0x0312;
        private const int HOTKEY_ID = 0xBEEF;

        public event EventHandler? HotkeyPressed;

        public HotkeyHostForm()
        {
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Opacity = 0;
            Width = 1;
            Height = 1;
            StartPosition = FormStartPosition.Manual;
            Location = new Point(-32000, -32000);
            Load += (s, e) => { Visible = false; };
        }

        public void RegisterHotkey(uint modifiers, uint key, bool showError)
        {
            try { UnregisterHotKey(this.Handle, HOTKEY_ID); } catch { }

            if (key == 0) return;

            bool ok = RegisterHotKey(this.Handle, HOTKEY_ID, modifiers, key);
            if (!ok && showError)
            {
                MessageBox.Show(
                    "Failed to register the hotkey.\n\nThat combo may already be used by another program.\nPick a different one in Settings.",
                    "LightFlip",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY && m.WParam.ToInt32() == HOTKEY_ID)
                HotkeyPressed?.Invoke(this, EventArgs.Empty);

            base.WndProc(ref m);
        }

        public void SafeClose()
        {
            try { UnregisterHotKey(this.Handle, HOTKEY_ID); } catch { }
            try { Close(); } catch { }
            try { Dispose(); } catch { }
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}
