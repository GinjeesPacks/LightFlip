using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;

namespace LightFlip
{
    public partial class Form1 : Form
    {
        private AppConfig working;
        public AppConfig ResultConfig { get; private set; }

        internal bool AllowHardClose { get; set; } = false;

        public event EventHandler<AppConfig>? ConfigApplied;

        private uint capturedMods;
        private uint capturedKey;

        // NEW: per-game hotkey capture
        private uint capturedGameMods;
        private uint capturedGameKey;

        private bool suppress = false;

        public Form1(AppConfig cfg)
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath) ?? this.Icon;
            ApplyDarkTheme();

            working = Clone(cfg);
            ResultConfig = cfg;

            capturedMods = working.HotkeyModifiers;
            capturedKey = working.HotkeyKey;

            HookEvents();

            mnuMinimizeToTray.Checked = working.MinimizeToTray;
            mnuStartWithWindows.Checked = working.StartWithWindows;
            mnuStartMinimized.Checked = working.StartMinimized;

            // Start per-game hotkey controls disabled until a profile is selected/loaded
            txtGameHotkey.Enabled = false;
            btnClearGameHotkey.Enabled = false;

            RefreshGamesCombo(initial: true);
            UpdateHotkeyText();
        }

        private void HookEvents()
        {

            // Global hotkey capture
            txtHotkey.KeyDown += TxtHotkey_KeyDown;
            txtHotkey.KeyUp += (s, e) => { e.Handled = true; e.SuppressKeyPress = true; };
            txtHotkey.MouseDown += (s, e) => { txtHotkey.Focus(); };

            // Per-game hotkey capture
            txtGameHotkey.KeyDown += TxtGameHotkey_KeyDown;
            txtGameHotkey.KeyUp += (s, e) => { e.Handled = true; e.SuppressKeyPress = true; };
            txtGameHotkey.MouseDown += (s, e) => { txtGameHotkey.Focus(); };

            chkUseGameHotkey.CheckedChanged += (s, e) =>
            {
                bool on = chkUseGameHotkey.Checked;

                txtGameHotkey.Enabled = on;
                btnClearGameHotkey.Enabled = on;

                if (!on)
                {
                    capturedGameMods = 0;
                    capturedGameKey = 0;
                    UpdateGameHotkeyText();
                }
            };

            btnClearGameHotkey.Click += (s, e) =>
            {
                capturedGameMods = 0;
                capturedGameKey = 0;
                UpdateGameHotkeyText();
            };

            btnBrowseExe.Click += (s, e) => BrowseExe();
            btnSaveGame.Click += (s, e) => SaveAllFromForm(showSavedMessage: true);
            btnRemoveGame.Click += (s, e) => RemoveCurrentGame();

            cmbGames.SelectedIndexChanged += (s, e) => LoadSelectedGameToUi();

            chkRevertOnClose.CheckedChanged += (s, e) =>
            {
                var g = GetSelectedGame();
                if (g == null) return;
                g.RevertOnClose = chkRevertOnClose.Checked;
            };

            mnuMinimizeToTray.CheckedChanged += (s, e) => working.MinimizeToTray = mnuMinimizeToTray.Checked;
            mnuStartMinimized.CheckedChanged += (s, e) => working.StartMinimized = mnuStartMinimized.Checked;

            mnuStartWithWindows.CheckedChanged += (s, e) =>
            {
                working.StartWithWindows = mnuStartWithWindows.Checked;
            };

            btnClearHotkey.Click += (s, e) =>
            {
                capturedMods = 0;
                capturedKey = 0;
                UpdateHotkeyText();
            };

            btnOk.Click += (s, e) =>
            {
                if (!SaveAllFromForm(showSavedMessage: false))
                    return;

                Close();
            };

            btnCancel.Click += (s, e) => Close();
        }

        private void TxtHotkey_KeyDown(object? sender, KeyEventArgs e)
        {
            e.Handled = true;
            e.SuppressKeyPress = true;

            if (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.Menu ||
                e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin)
            {
                return;
            }

            uint mods = 0;
            if (e.Control) mods |= HotkeyMods.MOD_CONTROL;
            if (e.Alt) mods |= HotkeyMods.MOD_ALT;
            if (e.Shift) mods |= HotkeyMods.MOD_SHIFT;

            capturedMods = mods | HotkeyMods.MOD_NOREPEAT;
            capturedKey = (uint)e.KeyCode;

            UpdateHotkeyText();
        }

        // NEW: per-game hotkey capture
        private void TxtGameHotkey_KeyDown(object? sender, KeyEventArgs e)
        {
            e.Handled = true;
            e.SuppressKeyPress = true;

            if (!chkUseGameHotkey.Checked)
                return;

            if (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.Menu ||
                e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin)
            {
                return;
            }

            uint mods = 0;
            if (e.Control) mods |= HotkeyMods.MOD_CONTROL;
            if (e.Alt) mods |= HotkeyMods.MOD_ALT;
            if (e.Shift) mods |= HotkeyMods.MOD_SHIFT;

            capturedGameMods = mods | HotkeyMods.MOD_NOREPEAT;
            capturedGameKey = (uint)e.KeyCode;

            UpdateGameHotkeyText();
        }

        private void UpdateHotkeyText()
        {
            txtHotkey.Text = HotkeyText.Format(capturedMods, capturedKey);
        }

        // NEW
        private void UpdateGameHotkeyText()
        {
            txtGameHotkey.Text = HotkeyText.Format(capturedGameMods, capturedGameKey);
        }

        private void RefreshGamesCombo(bool initial)
        {
            suppress = true;

            string selectedExe = working.LastSelectedGameExePath ?? string.Empty;

            cmbGames.BeginUpdate();
            try
            {
                cmbGames.Items.Clear();

                foreach (var g in working.Games)
                    cmbGames.Items.Add(new GameComboItem(g));

                if (cmbGames.Items.Count > 0)
                {
                    int selectedIndex = 0;

                    if (!string.IsNullOrWhiteSpace(selectedExe))
                    {
                        for (int i = 0; i < cmbGames.Items.Count; i++)
                        {
                            if (cmbGames.Items[i] is GameComboItem item)
                            {
                                string exe = item.Profile.ExePath ?? string.Empty;
                                if (string.Equals(exe, selectedExe, StringComparison.OrdinalIgnoreCase))
                                {
                                    selectedIndex = i;
                                    break;
                                }
                            }
                        }
                    }

                    cmbGames.SelectedIndex = Math.Max(0, selectedIndex);
                }
                else
                {
                    ClearGameEditor();
                }
            }
            finally
            {
                cmbGames.EndUpdate();
                suppress = false;
            }

            if (initial)
                LoadSelectedGameToUi();
        }

        private void LoadSelectedGameToUi()
        {
            if (suppress) return;

            var g = GetSelectedGame();
            if (g == null)
            {
                ClearGameEditor();
                return;
            }

            working.LastSelectedGameExePath = g.ExePath;

            txtExePath.Text = g.ExePath;
            txtGameName.Text = g.DisplayName;

            numNormalBrightness.Value = ClampToRange((decimal)g.Normal.Brightness, numNormalBrightness.Minimum, numNormalBrightness.Maximum);
            numNormalContrast.Value = ClampToRange((decimal)g.Normal.Contrast, numNormalContrast.Minimum, numNormalContrast.Maximum);
            numNormalGamma.Value = ClampToRange((decimal)g.Normal.Gamma, numNormalGamma.Minimum, numNormalGamma.Maximum);

            numBrightBrightness.Value = ClampToRange((decimal)g.Bright.Brightness, numBrightBrightness.Minimum, numBrightBrightness.Maximum);
            numBrightContrast.Value = ClampToRange((decimal)g.Bright.Contrast, numBrightContrast.Minimum, numBrightContrast.Maximum);
            numBrightGamma.Value = ClampToRange((decimal)g.Bright.Gamma, numBrightGamma.Minimum, numBrightGamma.Maximum);

            chkRevertOnClose.Checked = g.RevertOnClose;

            // NEW: load per-game hotkey
            capturedGameMods = g.HotkeyModifiers;
            capturedGameKey = g.HotkeyKey;

            chkUseGameHotkey.Checked = (capturedGameKey != 0);
            txtGameHotkey.Enabled = chkUseGameHotkey.Checked;
            btnClearGameHotkey.Enabled = chkUseGameHotkey.Checked;

            UpdateGameHotkeyText();

            if (trackBar1 != null)
                trackBar1.Value = ClampToIntRange(g.Normal?.Temperature ?? 0f, trackBar1.Minimum, trackBar1.Maximum);

            if (trackBar2 != null)
                trackBar2.Value = ClampToIntRange(g.Bright?.Temperature ?? 0f, trackBar2.Minimum, trackBar2.Maximum);

        }

        private static int ClampToIntRange(float value, int min, int max)
        {
            int v = (int)Math.Round(value, MidpointRounding.AwayFromZero);
            if (v < min) return min;
            if (v > max) return max;
            return v;
        }


        private void ClearGameEditor()
        {
            txtExePath.Text = "";
            txtGameName.Text = "";

            numNormalBrightness.Value = 50;
            numNormalContrast.Value = 50;
            numNormalGamma.Value = 1.00m;

            numBrightBrightness.Value = 50;
            numBrightContrast.Value = 50;
            numBrightGamma.Value = 1.00m;

            chkRevertOnClose.Checked = true;

            // NEW: reset per-game hotkey UI
            capturedGameMods = 0;
            capturedGameKey = 0;

            chkUseGameHotkey.Checked = false;
            txtGameHotkey.Enabled = false;
            btnClearGameHotkey.Enabled = false;

            UpdateGameHotkeyText();

            // NEW: reset sliders
            if (trackBar1 != null) trackBar1.Value = 0;
            if (trackBar2 != null) trackBar2.Value = 0;
        }

        private GameProfile? GetSelectedGame()
        {
            if (cmbGames.SelectedItem is GameComboItem item)
                return item.Profile;

            return null;
        }

        private void BrowseExe()
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "Select game executable";
                ofd.Filter = "Executable (*.exe)|*.exe";
                ofd.CheckFileExists = true;
                ofd.CheckPathExists = true;

                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    string newExe = ofd.FileName;
                    string oldExe = (txtExePath.Text ?? "").Trim();

                    txtExePath.Text = newExe;

                    // Only update the name if the exe actually changed
                    if (!string.Equals(newExe, oldExe, StringComparison.OrdinalIgnoreCase))
                        txtGameName.Text = Path.GetFileNameWithoutExtension(newExe);
                }
            }
        }

        private bool SaveAllFromForm(bool showSavedMessage)
        {

            if (!SaveOrUpdateCurrentGameProfile())
                return false;

            working.HotkeyModifiers = capturedMods;
            working.HotkeyKey = capturedKey;

            working.MinimizeToTray = mnuMinimizeToTray.Checked;
            working.StartWithWindows = mnuStartWithWindows.Checked;
            working.StartMinimized = mnuStartMinimized.Checked;

            if (working.Games.Count > 0 && string.IsNullOrWhiteSpace(working.LastSelectedGameExePath))
                working.LastSelectedGameExePath = working.Games[0].ExePath;

            StartupManager.Apply(working.StartWithWindows);

            ResultConfig = working;

            try
            {
                working.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to save config.\n\n" + ex.Message, "LightFlip", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try { ConfigApplied?.Invoke(this, working); } catch { }

            RefreshGamesCombo(initial: false);

            if (showSavedMessage)
                MessageBox.Show(this, "Saved.", "LightFlip", MessageBoxButtons.OK, MessageBoxIcon.Information);

            return true;
        }

        private bool SaveOrUpdateCurrentGameProfile()
        {
            string exe = txtExePath.Text.Trim();

            if (string.IsNullOrWhiteSpace(exe))
                return true;

            if (!File.Exists(exe))
            {
                MessageBox.Show(this, "Pick a valid game .exe path first.", "LightFlip", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            string name = txtGameName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
                name = Path.GetFileNameWithoutExtension(exe);

            var existing = working.Games.FirstOrDefault(g => string.Equals(g.ExePath, exe, StringComparison.OrdinalIgnoreCase));
            if (existing == null)
            {
                existing = new GameProfile();
                working.Games.Add(existing);
            }

            existing.ExePath = exe;
            existing.DisplayName = name;

            existing.Normal = new ColorProfile
            {
                Brightness = (float)numNormalBrightness.Value,
                Contrast = (float)numNormalContrast.Value,
                Gamma = (float)numNormalGamma.Value,
                Temperature = (trackBar1 != null) ? trackBar1.Value : 0
            };

            existing.Bright = new ColorProfile
            {
                Brightness = (float)numBrightBrightness.Value,
                Contrast = (float)numBrightContrast.Value,
                Gamma = (float)numBrightGamma.Value,
                Temperature = (trackBar2 != null) ? trackBar2.Value : 0
            };

            existing.RevertOnClose = chkRevertOnClose.Checked;

            // NEW: save per-game hotkey (0/0 means "use global")
            if (chkUseGameHotkey.Checked && capturedGameKey != 0)
            {
                existing.HotkeyModifiers = capturedGameMods;
                existing.HotkeyKey = capturedGameKey;
            }
            else
            {
                existing.HotkeyModifiers = 0;
                existing.HotkeyKey = 0;
            }

            working.LastSelectedGameExePath = exe;

            return true;
        }

        private void RemoveCurrentGame()
        {
            var g = GetSelectedGame();
            if (g == null) return;

            var res = MessageBox.Show(this, $"Remove '{g.DisplayName}'?", "LightFlip", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res != DialogResult.Yes) return;

            working.Games.RemoveAll(x => string.Equals(x.ExePath, g.ExePath, StringComparison.OrdinalIgnoreCase));

            if (string.Equals(working.LastSelectedGameExePath, g.ExePath, StringComparison.OrdinalIgnoreCase))
                working.LastSelectedGameExePath = "";

            RefreshGamesCombo(initial: false);
        }

        private AppConfig Clone(AppConfig cfg)
        {
            var copy = new AppConfig
            {
                HotkeyModifiers = cfg.HotkeyModifiers,
                HotkeyKey = cfg.HotkeyKey,
                LastSelectedGameExePath = cfg.LastSelectedGameExePath ?? "",
                MinimizeToTray = cfg.MinimizeToTray,
                StartWithWindows = cfg.StartWithWindows,
                StartMinimized = cfg.StartMinimized,
                Games = cfg.Games.Select(g => new GameProfile
                {
                    ExePath = g.ExePath ?? "",
                    DisplayName = g.DisplayName ?? "",
                    LastBright = g.LastBright,
                    RevertOnClose = g.RevertOnClose,

                    // NEW: clone per-game hotkey
                    HotkeyModifiers = g.HotkeyModifiers,
                    HotkeyKey = g.HotkeyKey,

                    Normal = new ColorProfile
                    {
                        Brightness = g.Normal?.Brightness ?? 50,
                        Contrast = g.Normal?.Contrast ?? 50,
                        Gamma = g.Normal?.Gamma ?? 1.00f,
                        Temperature = g.Normal?.Temperature ?? 0
                    },
                    Bright = new ColorProfile
                    {
                        Brightness = g.Bright?.Brightness ?? 50,
                        Contrast = g.Bright?.Contrast ?? 50,
                        Gamma = g.Bright?.Gamma ?? 1.00f,
                        Temperature = g.Bright?.Temperature ?? 0
                    }
                }).ToList()
            };

            return copy;
        }

        private decimal ClampToRange(decimal value, decimal min, decimal max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        // NEW: helper for trackbar int values
        private int ClampToIntRange(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        private void ApplyDarkTheme()
        {
            SuspendLayout();
            try
            {
                BackColor = Color.FromArgb(24, 24, 24);
                ForeColor = Color.Gainsboro;

                ApplyDarkThemeToControl(this);
                ApplyDarkThemeToToolStrips(this);
            }
            finally
            {
                ResumeLayout(true);
            }
        }

        private void ApplyDarkThemeToControl(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is TextBoxBase)
                {
                    c.BackColor = Color.FromArgb(32, 32, 32);
                    c.ForeColor = Color.Gainsboro;
                }
                else if (c is ComboBox)
                {
                    c.BackColor = Color.FromArgb(32, 32, 32);
                    c.ForeColor = Color.Gainsboro;
                }
                else if (c is NumericUpDown)
                {
                    c.BackColor = Color.FromArgb(32, 32, 32);
                    c.ForeColor = Color.Gainsboro;
                }
                else if (c is ListBox)
                {
                    c.BackColor = Color.FromArgb(32, 32, 32);
                    c.ForeColor = Color.Gainsboro;
                }
                else if (c is Button)
                {
                    c.BackColor = Color.FromArgb(45, 45, 45);
                    c.ForeColor = Color.Gainsboro;
                    ((Button)c).FlatStyle = FlatStyle.Flat;
                    ((Button)c).FlatAppearance.BorderColor = Color.FromArgb(70, 70, 70);
                }
                else if (c is CheckBox || c is RadioButton || c is Label || c is GroupBox)
                {
                    c.ForeColor = Color.Gainsboro;

                    if (c is GroupBox)
                        c.BackColor = Color.Transparent;
                }
                else if (c is Panel || c is FlowLayoutPanel || c is TableLayoutPanel)
                {
                    c.BackColor = Color.Transparent;
                    c.ForeColor = Color.Gainsboro;
                }
                else
                {
                    if (c.BackColor == SystemColors.Control || c.BackColor == SystemColors.Window)
                        c.BackColor = Color.FromArgb(24, 24, 24);

                    c.ForeColor = Color.Gainsboro;
                }

                if (c.HasChildren)
                    ApplyDarkThemeToControl(c);
            }
        }

        private void ApplyDarkThemeToToolStrips(Control root)
        {
            foreach (Control c in root.Controls)
            {
                if (c is MenuStrip ms)
                    ThemeToolStrip(ms);

                if (c is ToolStrip ts)
                    ThemeToolStrip(ts);

                if (c is StatusStrip ss)
                    ThemeToolStrip(ss);

                if (c.HasChildren)
                    ApplyDarkThemeToToolStrips(c);
            }
        }

        private void ThemeToolStrip(ToolStrip ts)
        {
            ts.Renderer = new DarkToolStripRenderer();
            ts.BackColor = Color.FromArgb(28, 28, 28);
            ts.ForeColor = Color.Gainsboro;

            foreach (ToolStripItem item in ts.Items)
            {
                item.BackColor = Color.FromArgb(28, 28, 28);
                item.ForeColor = Color.Gainsboro;

                if (item is ToolStripDropDownItem dd)
                    ThemeToolStripDropDown(dd.DropDown);
            }
        }

        private void ThemeToolStripDropDown(ToolStripDropDown dd)
        {
            dd.Renderer = new DarkToolStripRenderer();
            dd.BackColor = Color.FromArgb(28, 28, 28);
            dd.ForeColor = Color.Gainsboro;

            foreach (ToolStripItem item in dd.Items)
            {
                item.BackColor = Color.FromArgb(28, 28, 28);
                item.ForeColor = Color.Gainsboro;

                if (item is ToolStripDropDownItem child)
                    ThemeToolStripDropDown(child.DropDown);
            }
        }

        private sealed class DarkToolStripRenderer : ToolStripProfessionalRenderer
        {
            protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
            {
                e.Graphics.Clear(Color.FromArgb(28, 28, 28));
            }

            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
            {
                Color bg = e.Item.Selected
                    ? Color.FromArgb(55, 55, 55)
                    : Color.FromArgb(28, 28, 28);

                using (var b = new SolidBrush(bg))
                    e.Graphics.FillRectangle(b, new Rectangle(Point.Empty, e.Item.Size));
            }

            protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
            {
                int y = e.Item.ContentRectangle.Top + (e.Item.ContentRectangle.Height / 2);
                using (var p = new Pen(Color.FromArgb(70, 70, 70)))
                    e.Graphics.DrawLine(p, e.Item.ContentRectangle.Left, y, e.Item.ContentRectangle.Right, y);
            }
        }

        private sealed class GameComboItem
        {
            public GameProfile Profile { get; }

            public GameComboItem(GameProfile p)
            {
                Profile = p;
            }

            public override string ToString()
            {
                if (!string.IsNullOrWhiteSpace(Profile.DisplayName))
                    return Profile.DisplayName;
                if (!string.IsNullOrWhiteSpace(Profile.ExePath))
                    return Path.GetFileNameWithoutExtension(Profile.ExePath);
                return "(Unknown)";
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
