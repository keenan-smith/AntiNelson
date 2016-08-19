namespace PointBlank_GUI
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "CPU Usage",
            "Calculating..."}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "Memory Usage",
            "Calculating..."}, -1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "Network Usage",
            "Calculating..."}, -1);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "Disk Usage",
            "Calculating..."}, -1);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPlayers = new System.Windows.Forms.TabPage();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.tabConsole = new System.Windows.Forms.TabPage();
            this.tabPlugins = new System.Windows.Forms.TabPage();
            this.tabMods = new System.Windows.Forms.TabPage();
            this.tabUsage = new System.Windows.Forms.TabPage();
            this.listView1 = new System.Windows.Forms.ListView();
            this.colPlayerName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colACDetetction = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colWarnings = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colKills = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDeaths = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPing = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabOffline = new System.Windows.Forms.TabPage();
            this.listView2 = new System.Windows.Forms.ListView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.colNames = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabServer = new System.Windows.Forms.TabPage();
            this.listView3 = new System.Windows.Forms.ListView();
            this.colPNames = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colACDetections = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colWarns = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colKill = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDeath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colBanned = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnSRestart = new System.Windows.Forms.Button();
            this.btnSStop = new System.Windows.Forms.Button();
            this.btnSStart = new System.Windows.Forms.Button();
            this.groupServer = new System.Windows.Forms.GroupBox();
            this.groupPlugins = new System.Windows.Forms.GroupBox();
            this.btnPReloadS = new System.Windows.Forms.Button();
            this.btnPReloadP = new System.Windows.Forms.Button();
            this.btnPRefresh = new System.Windows.Forms.Button();
            this.groupMods = new System.Windows.Forms.GroupBox();
            this.btnMReloadS = new System.Windows.Forms.Button();
            this.btnMReloadM = new System.Windows.Forms.Button();
            this.btnMRefresh = new System.Windows.Forms.Button();
            this.listView4 = new System.Windows.Forms.ListView();
            this.colPlugin = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSetting = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listView5 = new System.Windows.Forms.ListView();
            this.colPluginName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPluginVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPluginActive = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listView6 = new System.Windows.Forms.ListView();
            this.colModName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colModVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colModActive = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listView7 = new System.Windows.Forms.ListView();
            this.colMonitorName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMonitorUsage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl1.SuspendLayout();
            this.tabPlayers.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.tabConsole.SuspendLayout();
            this.tabPlugins.SuspendLayout();
            this.tabMods.SuspendLayout();
            this.tabUsage.SuspendLayout();
            this.tabOffline.SuspendLayout();
            this.tabServer.SuspendLayout();
            this.groupServer.SuspendLayout();
            this.groupPlugins.SuspendLayout();
            this.groupMods.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabConsole);
            this.tabControl1.Controls.Add(this.tabServer);
            this.tabControl1.Controls.Add(this.tabPlayers);
            this.tabControl1.Controls.Add(this.tabOffline);
            this.tabControl1.Controls.Add(this.tabSettings);
            this.tabControl1.Controls.Add(this.tabPlugins);
            this.tabControl1.Controls.Add(this.tabMods);
            this.tabControl1.Controls.Add(this.tabUsage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(642, 385);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPlayers
            // 
            this.tabPlayers.Controls.Add(this.listView1);
            this.tabPlayers.Location = new System.Drawing.Point(4, 22);
            this.tabPlayers.Name = "tabPlayers";
            this.tabPlayers.Size = new System.Drawing.Size(634, 359);
            this.tabPlayers.TabIndex = 1;
            this.tabPlayers.Text = "Players";
            this.tabPlayers.UseVisualStyleBackColor = true;
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.listView4);
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Size = new System.Drawing.Size(634, 359);
            this.tabSettings.TabIndex = 2;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // tabConsole
            // 
            this.tabConsole.Controls.Add(this.richTextBox1);
            this.tabConsole.Controls.Add(this.textBox1);
            this.tabConsole.Controls.Add(this.listView2);
            this.tabConsole.Location = new System.Drawing.Point(4, 22);
            this.tabConsole.Name = "tabConsole";
            this.tabConsole.Size = new System.Drawing.Size(634, 359);
            this.tabConsole.TabIndex = 3;
            this.tabConsole.Text = "Console";
            this.tabConsole.UseVisualStyleBackColor = true;
            // 
            // tabPlugins
            // 
            this.tabPlugins.Controls.Add(this.listView5);
            this.tabPlugins.Location = new System.Drawing.Point(4, 22);
            this.tabPlugins.Name = "tabPlugins";
            this.tabPlugins.Size = new System.Drawing.Size(634, 359);
            this.tabPlugins.TabIndex = 4;
            this.tabPlugins.Text = "Plugins";
            this.tabPlugins.UseVisualStyleBackColor = true;
            // 
            // tabMods
            // 
            this.tabMods.Controls.Add(this.listView6);
            this.tabMods.Location = new System.Drawing.Point(4, 22);
            this.tabMods.Name = "tabMods";
            this.tabMods.Size = new System.Drawing.Size(634, 359);
            this.tabMods.TabIndex = 5;
            this.tabMods.Text = "Mods";
            this.tabMods.UseVisualStyleBackColor = true;
            // 
            // tabUsage
            // 
            this.tabUsage.Controls.Add(this.listView7);
            this.tabUsage.Location = new System.Drawing.Point(4, 22);
            this.tabUsage.Name = "tabUsage";
            this.tabUsage.Size = new System.Drawing.Size(634, 359);
            this.tabUsage.TabIndex = 6;
            this.tabUsage.Text = "System Usage";
            this.tabUsage.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colPlayerName,
            this.colACDetetction,
            this.colWarnings,
            this.colKills,
            this.colDeaths,
            this.colPing});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(634, 359);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // colPlayerName
            // 
            this.colPlayerName.Text = "Player Name";
            this.colPlayerName.Width = 255;
            // 
            // colACDetetction
            // 
            this.colACDetetction.Text = "AntiCheat Detections";
            this.colACDetetction.Width = 113;
            // 
            // colWarnings
            // 
            this.colWarnings.Text = "Warnings";
            this.colWarnings.Width = 62;
            // 
            // colKills
            // 
            this.colKills.Text = "Kills";
            // 
            // colDeaths
            // 
            this.colDeaths.Text = "Deaths";
            // 
            // colPing
            // 
            this.colPing.Text = "Ping";
            // 
            // tabOffline
            // 
            this.tabOffline.Controls.Add(this.listView3);
            this.tabOffline.Location = new System.Drawing.Point(4, 22);
            this.tabOffline.Name = "tabOffline";
            this.tabOffline.Size = new System.Drawing.Size(634, 359);
            this.tabOffline.TabIndex = 7;
            this.tabOffline.Text = "Offline Players";
            this.tabOffline.UseVisualStyleBackColor = true;
            // 
            // listView2
            // 
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colNames});
            this.listView2.Dock = System.Windows.Forms.DockStyle.Right;
            this.listView2.FullRowSelect = true;
            this.listView2.Location = new System.Drawing.Point(500, 0);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(134, 359);
            this.listView2.TabIndex = 2;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox1.Location = new System.Drawing.Point(0, 339);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(500, 20);
            this.textBox1.TabIndex = 3;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBox1.Size = new System.Drawing.Size(500, 339);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "";
            // 
            // colNames
            // 
            this.colNames.Text = "Players";
            this.colNames.Width = 129;
            // 
            // tabServer
            // 
            this.tabServer.Controls.Add(this.groupMods);
            this.tabServer.Controls.Add(this.groupPlugins);
            this.tabServer.Controls.Add(this.groupServer);
            this.tabServer.Location = new System.Drawing.Point(4, 22);
            this.tabServer.Name = "tabServer";
            this.tabServer.Size = new System.Drawing.Size(634, 359);
            this.tabServer.TabIndex = 8;
            this.tabServer.Text = "Server";
            this.tabServer.UseVisualStyleBackColor = true;
            // 
            // listView3
            // 
            this.listView3.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colPNames,
            this.colACDetections,
            this.colWarns,
            this.colKill,
            this.colDeath,
            this.colBanned});
            this.listView3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView3.FullRowSelect = true;
            this.listView3.Location = new System.Drawing.Point(0, 0);
            this.listView3.Name = "listView3";
            this.listView3.Size = new System.Drawing.Size(634, 359);
            this.listView3.TabIndex = 1;
            this.listView3.UseCompatibleStateImageBehavior = false;
            this.listView3.View = System.Windows.Forms.View.Details;
            // 
            // colPNames
            // 
            this.colPNames.Text = "Player Name";
            this.colPNames.Width = 255;
            // 
            // colACDetections
            // 
            this.colACDetections.Text = "AntiCheat Detections";
            this.colACDetections.Width = 113;
            // 
            // colWarns
            // 
            this.colWarns.Text = "Warnings";
            this.colWarns.Width = 62;
            // 
            // colKill
            // 
            this.colKill.Text = "Kills";
            // 
            // colDeath
            // 
            this.colDeath.Text = "Deaths";
            // 
            // colBanned
            // 
            this.colBanned.Text = "Banned";
            this.colBanned.Width = 76;
            // 
            // btnSRestart
            // 
            this.btnSRestart.Location = new System.Drawing.Point(6, 48);
            this.btnSRestart.Name = "btnSRestart";
            this.btnSRestart.Size = new System.Drawing.Size(151, 23);
            this.btnSRestart.TabIndex = 0;
            this.btnSRestart.Text = "Restart Server";
            this.btnSRestart.UseVisualStyleBackColor = true;
            // 
            // btnSStop
            // 
            this.btnSStop.Location = new System.Drawing.Point(6, 77);
            this.btnSStop.Name = "btnSStop";
            this.btnSStop.Size = new System.Drawing.Size(151, 23);
            this.btnSStop.TabIndex = 1;
            this.btnSStop.Text = "Stop Server";
            this.btnSStop.UseVisualStyleBackColor = true;
            // 
            // btnSStart
            // 
            this.btnSStart.Location = new System.Drawing.Point(6, 19);
            this.btnSStart.Name = "btnSStart";
            this.btnSStart.Size = new System.Drawing.Size(151, 23);
            this.btnSStart.TabIndex = 2;
            this.btnSStart.Text = "Start Server";
            this.btnSStart.UseVisualStyleBackColor = true;
            // 
            // groupServer
            // 
            this.groupServer.Controls.Add(this.btnSStart);
            this.groupServer.Controls.Add(this.btnSRestart);
            this.groupServer.Controls.Add(this.btnSStop);
            this.groupServer.Location = new System.Drawing.Point(8, 14);
            this.groupServer.Name = "groupServer";
            this.groupServer.Size = new System.Drawing.Size(165, 121);
            this.groupServer.TabIndex = 3;
            this.groupServer.TabStop = false;
            this.groupServer.Text = "Managment";
            // 
            // groupPlugins
            // 
            this.groupPlugins.Controls.Add(this.btnPReloadS);
            this.groupPlugins.Controls.Add(this.btnPReloadP);
            this.groupPlugins.Controls.Add(this.btnPRefresh);
            this.groupPlugins.Location = new System.Drawing.Point(8, 141);
            this.groupPlugins.Name = "groupPlugins";
            this.groupPlugins.Size = new System.Drawing.Size(165, 121);
            this.groupPlugins.TabIndex = 4;
            this.groupPlugins.TabStop = false;
            this.groupPlugins.Text = "Plugins";
            // 
            // btnPReloadS
            // 
            this.btnPReloadS.Location = new System.Drawing.Point(6, 19);
            this.btnPReloadS.Name = "btnPReloadS";
            this.btnPReloadS.Size = new System.Drawing.Size(151, 23);
            this.btnPReloadS.TabIndex = 2;
            this.btnPReloadS.Text = "Reload System";
            this.btnPReloadS.UseVisualStyleBackColor = true;
            // 
            // btnPReloadP
            // 
            this.btnPReloadP.Location = new System.Drawing.Point(6, 48);
            this.btnPReloadP.Name = "btnPReloadP";
            this.btnPReloadP.Size = new System.Drawing.Size(151, 23);
            this.btnPReloadP.TabIndex = 0;
            this.btnPReloadP.Text = "Reload Plugins";
            this.btnPReloadP.UseVisualStyleBackColor = true;
            // 
            // btnPRefresh
            // 
            this.btnPRefresh.Location = new System.Drawing.Point(6, 77);
            this.btnPRefresh.Name = "btnPRefresh";
            this.btnPRefresh.Size = new System.Drawing.Size(151, 23);
            this.btnPRefresh.TabIndex = 1;
            this.btnPRefresh.Text = "Refresh Plugins";
            this.btnPRefresh.UseVisualStyleBackColor = true;
            // 
            // groupMods
            // 
            this.groupMods.Controls.Add(this.btnMReloadS);
            this.groupMods.Controls.Add(this.btnMReloadM);
            this.groupMods.Controls.Add(this.btnMRefresh);
            this.groupMods.Location = new System.Drawing.Point(179, 14);
            this.groupMods.Name = "groupMods";
            this.groupMods.Size = new System.Drawing.Size(165, 121);
            this.groupMods.TabIndex = 5;
            this.groupMods.TabStop = false;
            this.groupMods.Text = "Mods";
            // 
            // btnMReloadS
            // 
            this.btnMReloadS.Location = new System.Drawing.Point(6, 19);
            this.btnMReloadS.Name = "btnMReloadS";
            this.btnMReloadS.Size = new System.Drawing.Size(151, 23);
            this.btnMReloadS.TabIndex = 2;
            this.btnMReloadS.Text = "Reload System";
            this.btnMReloadS.UseVisualStyleBackColor = true;
            // 
            // btnMReloadM
            // 
            this.btnMReloadM.Location = new System.Drawing.Point(6, 48);
            this.btnMReloadM.Name = "btnMReloadM";
            this.btnMReloadM.Size = new System.Drawing.Size(151, 23);
            this.btnMReloadM.TabIndex = 0;
            this.btnMReloadM.Text = "Reload Mods";
            this.btnMReloadM.UseVisualStyleBackColor = true;
            // 
            // btnMRefresh
            // 
            this.btnMRefresh.Location = new System.Drawing.Point(6, 77);
            this.btnMRefresh.Name = "btnMRefresh";
            this.btnMRefresh.Size = new System.Drawing.Size(151, 23);
            this.btnMRefresh.TabIndex = 1;
            this.btnMRefresh.Text = "Refresh Mods";
            this.btnMRefresh.UseVisualStyleBackColor = true;
            // 
            // listView4
            // 
            this.listView4.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colPlugin,
            this.colSetting,
            this.colValue});
            this.listView4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView4.FullRowSelect = true;
            this.listView4.Location = new System.Drawing.Point(0, 0);
            this.listView4.Name = "listView4";
            this.listView4.Size = new System.Drawing.Size(634, 359);
            this.listView4.TabIndex = 0;
            this.listView4.UseCompatibleStateImageBehavior = false;
            this.listView4.View = System.Windows.Forms.View.Details;
            // 
            // colPlugin
            // 
            this.colPlugin.Text = "Plugin";
            this.colPlugin.Width = 127;
            // 
            // colSetting
            // 
            this.colSetting.Text = "Setting";
            this.colSetting.Width = 219;
            // 
            // colValue
            // 
            this.colValue.Text = "Value";
            this.colValue.Width = 281;
            // 
            // listView5
            // 
            this.listView5.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colPluginName,
            this.colPluginVersion,
            this.colPluginActive});
            this.listView5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView5.FullRowSelect = true;
            this.listView5.Location = new System.Drawing.Point(0, 0);
            this.listView5.Name = "listView5";
            this.listView5.Size = new System.Drawing.Size(634, 359);
            this.listView5.TabIndex = 0;
            this.listView5.UseCompatibleStateImageBehavior = false;
            this.listView5.View = System.Windows.Forms.View.Details;
            // 
            // colPluginName
            // 
            this.colPluginName.Text = "Plugin Name";
            this.colPluginName.Width = 384;
            // 
            // colPluginVersion
            // 
            this.colPluginVersion.Text = "Plugin Version";
            this.colPluginVersion.Width = 128;
            // 
            // colPluginActive
            // 
            this.colPluginActive.Text = "Active";
            this.colPluginActive.Width = 117;
            // 
            // listView6
            // 
            this.listView6.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colModName,
            this.colModVersion,
            this.colModActive});
            this.listView6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView6.FullRowSelect = true;
            this.listView6.Location = new System.Drawing.Point(0, 0);
            this.listView6.Name = "listView6";
            this.listView6.Size = new System.Drawing.Size(634, 359);
            this.listView6.TabIndex = 1;
            this.listView6.UseCompatibleStateImageBehavior = false;
            this.listView6.View = System.Windows.Forms.View.Details;
            // 
            // colModName
            // 
            this.colModName.Text = "Mod Name";
            this.colModName.Width = 384;
            // 
            // colModVersion
            // 
            this.colModVersion.Text = "Mod Version";
            this.colModVersion.Width = 128;
            // 
            // colModActive
            // 
            this.colModActive.Text = "Active";
            this.colModActive.Width = 117;
            // 
            // listView7
            // 
            this.listView7.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colMonitorName,
            this.colMonitorUsage});
            this.listView7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView7.FullRowSelect = true;
            this.listView7.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4});
            this.listView7.Location = new System.Drawing.Point(0, 0);
            this.listView7.Name = "listView7";
            this.listView7.Size = new System.Drawing.Size(634, 359);
            this.listView7.TabIndex = 0;
            this.listView7.UseCompatibleStateImageBehavior = false;
            this.listView7.View = System.Windows.Forms.View.Details;
            // 
            // colMonitorName
            // 
            this.colMonitorName.Text = "Monitor Name";
            this.colMonitorName.Width = 280;
            // 
            // colMonitorUsage
            // 
            this.colMonitorUsage.Text = "Usage";
            this.colMonitorUsage.Width = 349;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 385);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PointBlank Console";
            this.tabControl1.ResumeLayout(false);
            this.tabPlayers.ResumeLayout(false);
            this.tabSettings.ResumeLayout(false);
            this.tabConsole.ResumeLayout(false);
            this.tabConsole.PerformLayout();
            this.tabPlugins.ResumeLayout(false);
            this.tabMods.ResumeLayout(false);
            this.tabUsage.ResumeLayout(false);
            this.tabOffline.ResumeLayout(false);
            this.tabServer.ResumeLayout(false);
            this.groupServer.ResumeLayout(false);
            this.groupPlugins.ResumeLayout(false);
            this.groupMods.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabConsole;
        private System.Windows.Forms.TabPage tabPlayers;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.TabPage tabPlugins;
        private System.Windows.Forms.TabPage tabMods;
        private System.Windows.Forms.TabPage tabUsage;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader colPlayerName;
        private System.Windows.Forms.ColumnHeader colACDetetction;
        private System.Windows.Forms.ColumnHeader colWarnings;
        private System.Windows.Forms.ColumnHeader colKills;
        private System.Windows.Forms.ColumnHeader colDeaths;
        private System.Windows.Forms.ColumnHeader colPing;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.TabPage tabOffline;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ColumnHeader colNames;
        private System.Windows.Forms.TabPage tabServer;
        private System.Windows.Forms.ListView listView3;
        private System.Windows.Forms.ColumnHeader colPNames;
        private System.Windows.Forms.ColumnHeader colACDetections;
        private System.Windows.Forms.ColumnHeader colWarns;
        private System.Windows.Forms.ColumnHeader colKill;
        private System.Windows.Forms.ColumnHeader colDeath;
        private System.Windows.Forms.ColumnHeader colBanned;
        private System.Windows.Forms.Button btnSStart;
        private System.Windows.Forms.Button btnSStop;
        private System.Windows.Forms.Button btnSRestart;
        private System.Windows.Forms.GroupBox groupMods;
        private System.Windows.Forms.Button btnMReloadS;
        private System.Windows.Forms.Button btnMReloadM;
        private System.Windows.Forms.Button btnMRefresh;
        private System.Windows.Forms.GroupBox groupPlugins;
        private System.Windows.Forms.Button btnPReloadS;
        private System.Windows.Forms.Button btnPReloadP;
        private System.Windows.Forms.Button btnPRefresh;
        private System.Windows.Forms.GroupBox groupServer;
        private System.Windows.Forms.ListView listView4;
        private System.Windows.Forms.ColumnHeader colPlugin;
        private System.Windows.Forms.ColumnHeader colSetting;
        private System.Windows.Forms.ColumnHeader colValue;
        private System.Windows.Forms.ListView listView5;
        private System.Windows.Forms.ColumnHeader colPluginName;
        private System.Windows.Forms.ColumnHeader colPluginVersion;
        private System.Windows.Forms.ColumnHeader colPluginActive;
        private System.Windows.Forms.ListView listView6;
        private System.Windows.Forms.ColumnHeader colModName;
        private System.Windows.Forms.ColumnHeader colModVersion;
        private System.Windows.Forms.ColumnHeader colModActive;
        private System.Windows.Forms.ListView listView7;
        private System.Windows.Forms.ColumnHeader colMonitorName;
        private System.Windows.Forms.ColumnHeader colMonitorUsage;
    }
}

