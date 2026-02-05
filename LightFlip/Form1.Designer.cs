namespace LightFlip
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuOptions;
        internal System.Windows.Forms.ToolStripMenuItem mnuMinimizeToTray;
        internal System.Windows.Forms.ToolStripMenuItem mnuStartWithWindows;
        internal System.Windows.Forms.ToolStripMenuItem mnuStartMinimized;

        private System.Windows.Forms.GroupBox grpGames;
        internal System.Windows.Forms.ComboBox cmbGames;
        private System.Windows.Forms.Label lblGameSelect;
        private System.Windows.Forms.Label lblExe;
        internal System.Windows.Forms.TextBox txtExePath;
        internal System.Windows.Forms.Button btnBrowseExe;
        private System.Windows.Forms.Label lblName;
        internal System.Windows.Forms.TextBox txtGameName;
        internal System.Windows.Forms.CheckBox chkRevertOnClose;
        internal System.Windows.Forms.Button btnSaveGame;
        internal System.Windows.Forms.Button btnRemoveGame;

        private System.Windows.Forms.GroupBox grpNormal;
        internal System.Windows.Forms.NumericUpDown numNormalBrightness;
        internal System.Windows.Forms.NumericUpDown numNormalContrast;
        internal System.Windows.Forms.NumericUpDown numNormalGamma;

        private System.Windows.Forms.GroupBox grpBright;
        internal System.Windows.Forms.NumericUpDown numBrightBrightness;
        internal System.Windows.Forms.NumericUpDown numBrightContrast;
        internal System.Windows.Forms.NumericUpDown numBrightGamma;

        private System.Windows.Forms.GroupBox grpHotkey;
        internal System.Windows.Forms.TextBox txtHotkey;
        internal System.Windows.Forms.Button btnClearHotkey;

        internal System.Windows.Forms.Button btnOk;
        internal System.Windows.Forms.Button btnCancel;

        private System.Windows.Forms.Label lblNB;
        private System.Windows.Forms.Label lblNC;
        private System.Windows.Forms.Label lblNG;

        private System.Windows.Forms.Label lblBB;
        private System.Windows.Forms.Label lblBC;
        private System.Windows.Forms.Label lblBG;

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            menuStrip1 = new System.Windows.Forms.MenuStrip();
            mnuOptions = new System.Windows.Forms.ToolStripMenuItem();
            mnuMinimizeToTray = new System.Windows.Forms.ToolStripMenuItem();
            mnuStartWithWindows = new System.Windows.Forms.ToolStripMenuItem();
            mnuStartMinimized = new System.Windows.Forms.ToolStripMenuItem();

            grpGames = new System.Windows.Forms.GroupBox();
            cmbGames = new System.Windows.Forms.ComboBox();
            lblGameSelect = new System.Windows.Forms.Label();
            lblExe = new System.Windows.Forms.Label();
            txtExePath = new System.Windows.Forms.TextBox();
            btnBrowseExe = new System.Windows.Forms.Button();
            lblName = new System.Windows.Forms.Label();
            txtGameName = new System.Windows.Forms.TextBox();
            chkRevertOnClose = new System.Windows.Forms.CheckBox();
            btnSaveGame = new System.Windows.Forms.Button();
            btnRemoveGame = new System.Windows.Forms.Button();

            grpNormal = new System.Windows.Forms.GroupBox();
            numNormalBrightness = new System.Windows.Forms.NumericUpDown();
            numNormalContrast = new System.Windows.Forms.NumericUpDown();
            numNormalGamma = new System.Windows.Forms.NumericUpDown();
            lblNB = new System.Windows.Forms.Label();
            lblNC = new System.Windows.Forms.Label();
            lblNG = new System.Windows.Forms.Label();

            grpBright = new System.Windows.Forms.GroupBox();
            numBrightBrightness = new System.Windows.Forms.NumericUpDown();
            numBrightContrast = new System.Windows.Forms.NumericUpDown();
            numBrightGamma = new System.Windows.Forms.NumericUpDown();
            lblBB = new System.Windows.Forms.Label();
            lblBC = new System.Windows.Forms.Label();
            lblBG = new System.Windows.Forms.Label();

            grpHotkey = new System.Windows.Forms.GroupBox();
            txtHotkey = new System.Windows.Forms.TextBox();
            btnClearHotkey = new System.Windows.Forms.Button();

            btnOk = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();

            
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuOptions });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new System.Drawing.Size(820, 24);

            mnuOptions.Name = "mnuOptions";
            mnuOptions.Text = "Options";
            mnuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                mnuMinimizeToTray, mnuStartWithWindows, mnuStartMinimized
            });

            mnuMinimizeToTray.Text = "Minimize to tray (close hides)";
            mnuMinimizeToTray.CheckOnClick = true;

            mnuStartWithWindows.Text = "Start with Windows";
            mnuStartWithWindows.CheckOnClick = true;

            mnuStartMinimized.Text = "Start minimized to tray";
            mnuStartMinimized.CheckOnClick = true;

            
            grpGames.Text = "Game Profiles";
            grpGames.Location = new System.Drawing.Point(12, 34);
            grpGames.Size = new System.Drawing.Size(796, 150);

            lblGameSelect.Text = "Saved games:";
            lblGameSelect.Location = new System.Drawing.Point(14, 26);
            lblGameSelect.Size = new System.Drawing.Size(90, 18);

            cmbGames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbGames.Location = new System.Drawing.Point(110, 23);
            cmbGames.Size = new System.Drawing.Size(420, 23);

            btnRemoveGame.Text = "Remove";
            btnRemoveGame.Location = new System.Drawing.Point(540, 22);
            btnRemoveGame.Size = new System.Drawing.Size(90, 26);

            btnSaveGame.Text = "Save";
            btnSaveGame.Location = new System.Drawing.Point(636, 22);
            btnSaveGame.Size = new System.Drawing.Size(90, 26);

            lblExe.Text = "Game .exe:";
            lblExe.Location = new System.Drawing.Point(14, 58);
            lblExe.Size = new System.Drawing.Size(90, 18);

            txtExePath.Location = new System.Drawing.Point(110, 55);
            txtExePath.Size = new System.Drawing.Size(520, 23);

            btnBrowseExe.Text = "Browse";
            btnBrowseExe.Location = new System.Drawing.Point(636, 54);
            btnBrowseExe.Size = new System.Drawing.Size(90, 26);

            lblName.Text = "Name:";
            lblName.Location = new System.Drawing.Point(14, 90);
            lblName.Size = new System.Drawing.Size(90, 18);

            txtGameName.Location = new System.Drawing.Point(110, 87);
            txtGameName.Size = new System.Drawing.Size(420, 23);

            chkRevertOnClose.Text = "Revert to Normal on game close";
            chkRevertOnClose.Location = new System.Drawing.Point(110, 116);
            chkRevertOnClose.Size = new System.Drawing.Size(260, 20);

            grpGames.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblGameSelect, cmbGames, btnRemoveGame, btnSaveGame,
                lblExe, txtExePath, btnBrowseExe,
                lblName, txtGameName, chkRevertOnClose
            });

            
            grpNormal.Text = "Normal";
            grpNormal.Location = new System.Drawing.Point(12, 192);
            grpNormal.Size = new System.Drawing.Size(398, 170);

            grpBright.Text = "Bright";
            grpBright.Location = new System.Drawing.Point(410, 192);
            grpBright.Size = new System.Drawing.Size(398, 170);

            ConfigureProfileGroup(grpNormal, numNormalBrightness, numNormalContrast, numNormalGamma, lblNB, lblNC, lblNG);
            ConfigureProfileGroup(grpBright, numBrightBrightness, numBrightContrast, numBrightGamma, lblBB, lblBC, lblBG);

            
            grpHotkey.Text = "Hotkey";
            grpHotkey.Location = new System.Drawing.Point(12, 370);
            grpHotkey.Size = new System.Drawing.Size(796, 78);

            txtHotkey.ReadOnly = true;
            txtHotkey.Location = new System.Drawing.Point(16, 30);
            txtHotkey.Size = new System.Drawing.Size(640, 23);

            btnClearHotkey.Text = "Clear";
            btnClearHotkey.Location = new System.Drawing.Point(666, 28);
            btnClearHotkey.Size = new System.Drawing.Size(110, 26);

            grpHotkey.Controls.AddRange(new System.Windows.Forms.Control[] { txtHotkey, btnClearHotkey });

            
            btnCancel.Text = "Cancel";
            btnCancel.Location = new System.Drawing.Point(622, 460);
            btnCancel.Size = new System.Drawing.Size(90, 28);

            btnOk.Text = "OK";
            btnOk.Location = new System.Drawing.Point(718, 460);
            btnOk.Size = new System.Drawing.Size(90, 28);

            
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(820, 500);
            Controls.AddRange(new System.Windows.Forms.Control[] {
                menuStrip1, grpGames, grpNormal, grpBright, grpHotkey, btnCancel, btnOk
            });
            MainMenuStrip = menuStrip1;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = true;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "LightFlip";
        }

        private void ConfigureProfileGroup(
            System.Windows.Forms.GroupBox grp,
            System.Windows.Forms.NumericUpDown nudB,
            System.Windows.Forms.NumericUpDown nudC,
            System.Windows.Forms.NumericUpDown nudG,
            System.Windows.Forms.Label lblB,
            System.Windows.Forms.Label lblC,
            System.Windows.Forms.Label lblG)
        {
            lblB.Text = "Brightness";
            lblB.Location = new System.Drawing.Point(16, 32);
            lblB.Size = new System.Drawing.Size(80, 18);

            nudB.Minimum = 0;
            nudB.Maximum = 100;
            nudB.Value = 50;
            nudB.Location = new System.Drawing.Point(110, 30);
            nudB.Size = new System.Drawing.Size(90, 23);

            lblC.Text = "Contrast";
            lblC.Location = new System.Drawing.Point(16, 68);
            lblC.Size = new System.Drawing.Size(80, 18);

            nudC.Minimum = 0;
            nudC.Maximum = 100;
            nudC.Value = 50;
            nudC.Location = new System.Drawing.Point(110, 66);
            nudC.Size = new System.Drawing.Size(90, 23);

            lblG.Text = "Gamma";
            lblG.Location = new System.Drawing.Point(16, 104);
            lblG.Size = new System.Drawing.Size(80, 18);

            nudG.Minimum = 0.20m;
            nudG.Maximum = 5.00m;
            nudG.DecimalPlaces = 2;
            nudG.Increment = 0.01m;
            nudG.Value = 1.00m;
            nudG.Location = new System.Drawing.Point(110, 102);
            nudG.Size = new System.Drawing.Size(90, 23);

            grp.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblB, nudB, lblC, nudC, lblG, nudG
            });
        }
    }
}
