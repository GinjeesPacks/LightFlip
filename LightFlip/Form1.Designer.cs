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

        
        private System.Windows.Forms.Label lblGameHotkey;
        internal System.Windows.Forms.CheckBox chkUseGameHotkey;
        internal System.Windows.Forms.TextBox txtGameHotkey;
        internal System.Windows.Forms.Button btnClearGameHotkey;

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

        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.TrackBar trackBar2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;

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
            btnRemoveGame = new System.Windows.Forms.Button();
            btnSaveGame = new System.Windows.Forms.Button();
            lblExe = new System.Windows.Forms.Label();
            txtExePath = new System.Windows.Forms.TextBox();
            btnBrowseExe = new System.Windows.Forms.Button();
            lblName = new System.Windows.Forms.Label();
            txtGameName = new System.Windows.Forms.TextBox();

            lblGameHotkey = new System.Windows.Forms.Label();
            chkUseGameHotkey = new System.Windows.Forms.CheckBox();
            txtGameHotkey = new System.Windows.Forms.TextBox();
            btnClearGameHotkey = new System.Windows.Forms.Button();

            chkRevertOnClose = new System.Windows.Forms.CheckBox();

            grpNormal = new System.Windows.Forms.GroupBox();
            numNormalBrightness = new System.Windows.Forms.NumericUpDown();
            numNormalContrast = new System.Windows.Forms.NumericUpDown();
            numNormalGamma = new System.Windows.Forms.NumericUpDown();
            lblNB = new System.Windows.Forms.Label();
            lblNC = new System.Windows.Forms.Label();
            lblNG = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            trackBar1 = new System.Windows.Forms.TrackBar();

            grpBright = new System.Windows.Forms.GroupBox();
            numBrightBrightness = new System.Windows.Forms.NumericUpDown();
            numBrightContrast = new System.Windows.Forms.NumericUpDown();
            numBrightGamma = new System.Windows.Forms.NumericUpDown();
            lblBB = new System.Windows.Forms.Label();
            lblBC = new System.Windows.Forms.Label();
            lblBG = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            trackBar2 = new System.Windows.Forms.TrackBar();

            grpHotkey = new System.Windows.Forms.GroupBox();
            txtHotkey = new System.Windows.Forms.TextBox();
            btnClearHotkey = new System.Windows.Forms.Button();

            btnOk = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();

            menuStrip1.SuspendLayout();
            grpGames.SuspendLayout();

            grpNormal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numNormalBrightness).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numNormalContrast).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numNormalGamma).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();

            grpBright.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numBrightBrightness).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numBrightContrast).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numBrightGamma).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar2).BeginInit();

            grpHotkey.SuspendLayout();
            SuspendLayout();

            
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuOptions });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new System.Drawing.Size(820, 24);
            menuStrip1.TabIndex = 0;

             
            mnuOptions.Name = "mnuOptions";
            mnuOptions.Size = new System.Drawing.Size(61, 20);
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
            grpGames.Size = new System.Drawing.Size(796, 180);

            
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

             
            lblGameHotkey.Location = new System.Drawing.Point(14, 118);
            lblGameHotkey.Name = "lblGameHotkey";
            lblGameHotkey.Size = new System.Drawing.Size(100, 18);
            lblGameHotkey.TabIndex = 9;
            lblGameHotkey.Text = "Per-game hotkey:";

            
            chkUseGameHotkey.Location = new System.Drawing.Point(114, 118);
            chkUseGameHotkey.Name = "chkUseGameHotkey";
            chkUseGameHotkey.Size = new System.Drawing.Size(150, 20);
            chkUseGameHotkey.TabIndex = 10;
            chkUseGameHotkey.Text = "Use custom hotkey";

            
            txtGameHotkey.Location = new System.Drawing.Point(270, 114);
            txtGameHotkey.Name = "txtGameHotkey";
            txtGameHotkey.ReadOnly = true;
            txtGameHotkey.Size = new System.Drawing.Size(260, 23);
            txtGameHotkey.TabIndex = 11;

            
            btnClearGameHotkey.Location = new System.Drawing.Point(540, 112);
            btnClearGameHotkey.Name = "btnClearGameHotkey";
            btnClearGameHotkey.Size = new System.Drawing.Size(68, 26);
            btnClearGameHotkey.TabIndex = 12;
            btnClearGameHotkey.Text = "Clear";

            
            chkRevertOnClose.Text = "Revert to Normal on game close";
            chkRevertOnClose.Location = new System.Drawing.Point(16, 146);
            chkRevertOnClose.Size = new System.Drawing.Size(260, 20);

            grpGames.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblGameSelect, cmbGames, btnRemoveGame, btnSaveGame,
                lblExe, txtExePath, btnBrowseExe,
                lblName, txtGameName,
                lblGameHotkey, chkUseGameHotkey, txtGameHotkey, btnClearGameHotkey,
                chkRevertOnClose
            });

            
            grpNormal.Text = "Normal";
            grpNormal.Location = new System.Drawing.Point(12, 222);
            grpNormal.Size = new System.Drawing.Size(398, 170);

            
            ConfigureProfileGroup(grpNormal, numNormalBrightness, numNormalContrast, numNormalGamma, lblNB, lblNC, lblNG);

            
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(256, 47);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(104, 15);
            label2.TabIndex = 5;
            label2.Text = "(Cool \u2190  \u2192 Warm)";

            trackBar1.LargeChange = 10;
            trackBar1.Location = new System.Drawing.Point(240, 70);
            trackBar1.Maximum = 100;
            trackBar1.Minimum = -100;
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new System.Drawing.Size(134, 45);
            trackBar1.TabIndex = 2;
            trackBar1.TickFrequency = 25;

            grpNormal.Controls.Add(label2);
            grpNormal.Controls.Add(trackBar1);

            
            grpBright.Text = "Bright";
            grpBright.Location = new System.Drawing.Point(410, 222);
            grpBright.Size = new System.Drawing.Size(398, 170);

            
            ConfigureProfileGroup(grpBright, numBrightBrightness, numBrightContrast, numBrightGamma, lblBB, lblBC, lblBG);

            
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(254, 47);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(104, 15);
            label1.TabIndex = 4;
            label1.Text = "(Cool \u2190  \u2192 Warm)";

            trackBar2.LargeChange = 10;
            trackBar2.Location = new System.Drawing.Point(239, 71);
            trackBar2.Maximum = 100;
            trackBar2.Minimum = -100;
            trackBar2.Name = "trackBar2";
            trackBar2.Size = new System.Drawing.Size(134, 45);
            trackBar2.TabIndex = 3;
            trackBar2.TickFrequency = 25;

            grpBright.Controls.Add(label1);
            grpBright.Controls.Add(trackBar2);

            
            grpHotkey.Text = "Hotkey";
            grpHotkey.Location = new System.Drawing.Point(12, 400);
            grpHotkey.Size = new System.Drawing.Size(796, 78);

            txtHotkey.Location = new System.Drawing.Point(16, 30);
            txtHotkey.Name = "txtHotkey";
            txtHotkey.ReadOnly = true;
            txtHotkey.Size = new System.Drawing.Size(198, 23);
            txtHotkey.TabIndex = 0;

            btnClearHotkey.Location = new System.Drawing.Point(224, 29);
            btnClearHotkey.Name = "btnClearHotkey";
            btnClearHotkey.Size = new System.Drawing.Size(68, 26);
            btnClearHotkey.TabIndex = 1;
            btnClearHotkey.Text = "Clear";

            grpHotkey.Controls.Add(txtHotkey);
            grpHotkey.Controls.Add(btnClearHotkey);

            
            btnCancel.Location = new System.Drawing.Point(622, 490);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(90, 28);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Cancel";

            
            btnOk.Location = new System.Drawing.Point(718, 490);
            btnOk.Name = "btnOk";
            btnOk.Size = new System.Drawing.Size(90, 28);
            btnOk.TabIndex = 6;
            btnOk.Text = "OK";

            
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(820, 530);
            Controls.Add(menuStrip1);
            Controls.Add(grpGames);
            Controls.Add(grpNormal);
            Controls.Add(grpBright);
            Controls.Add(grpHotkey);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "LightFlip";

            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            grpGames.ResumeLayout(false);
            grpGames.PerformLayout();
            grpNormal.ResumeLayout(false);
            grpNormal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numNormalBrightness).EndInit();
            ((System.ComponentModel.ISupportInitialize)numNormalContrast).EndInit();
            ((System.ComponentModel.ISupportInitialize)numNormalGamma).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            grpBright.ResumeLayout(false);
            grpBright.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numBrightBrightness).EndInit();
            ((System.ComponentModel.ISupportInitialize)numBrightContrast).EndInit();
            ((System.ComponentModel.ISupportInitialize)numBrightGamma).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar2).EndInit();
            grpHotkey.ResumeLayout(false);
            grpHotkey.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
