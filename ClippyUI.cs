using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Advanced_Combat_Tracker;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Net.Http;

[assembly: AssemblyTitle("Clippy")]
[assembly: AssemblyDescription("A plugin for automatic creation of twitch clips upon wipe")]
[assembly: AssemblyCompany("Kiiroi Yuki")]
[assembly: AssemblyVersion("0.0.69.1")]

namespace Clippy
{
	public class ClippyUI : UserControl
	{
        public delegate void AddZoneDel(string zone);
        public delegate void RmZoneDel(string zone);
        public delegate void UpdateSettingsDel(string twitchChannel, string discordWebhook);
        public delegate Task StartTwitchAuthDel();
        public delegate Task<TwitchApi.ClipCreateResponse> CreateClipDel(string channel);

		#region Designer Created Code (Avoid editing)
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.Windows.Forms.SplitContainer SettingsLogSplitter;
            System.Windows.Forms.SplitContainer ActiveZonesSettingsSplitter;
            System.Windows.Forms.GroupBox EnabledZonesBox;
            System.Windows.Forms.FlowLayoutPanel EnabledZonesContainer;
            System.Windows.Forms.Panel CurrentZonePanel;
            System.Windows.Forms.Label CurrentZoneLabel;
            System.Windows.Forms.Panel ActiveZonesListGroup;
            System.Windows.Forms.Panel AddCustomZonePanel;
            System.Windows.Forms.GroupBox SettingsGroup;
            System.Windows.Forms.GroupBox TwitchConnectionGroup;
            System.Windows.Forms.Label TwitchConnectionStatusLabel;
            System.Windows.Forms.Label TargetChannelLabel;
            System.Windows.Forms.GroupBox LogGroupBox;
            System.Windows.Forms.Label DiscordWebhookLabel;
            this.addCurButton = new System.Windows.Forms.Button();
            this.curZoneText = new System.Windows.Forms.Label();
            this.removeButton = new System.Windows.Forms.Button();
            this.activeZonesList = new System.Windows.Forms.ListBox();
            this.addCustomZoneButton = new System.Windows.Forms.Button();
            this.addZoneTextBox = new System.Windows.Forms.TextBox();
            this.twitchAccountLinkButton = new System.Windows.Forms.Button();
            this.connectionStatusText = new System.Windows.Forms.Label();
            this.targetChannelTextBox = new System.Windows.Forms.TextBox();
            this.saveSettingsButton = new System.Windows.Forms.Button();
            this.logRTBox = new System.Windows.Forms.RichTextBox();
            this.discordWebhookTextBox = new System.Windows.Forms.TextBox();
            SettingsLogSplitter = new System.Windows.Forms.SplitContainer();
            ActiveZonesSettingsSplitter = new System.Windows.Forms.SplitContainer();
            EnabledZonesBox = new System.Windows.Forms.GroupBox();
            EnabledZonesContainer = new System.Windows.Forms.FlowLayoutPanel();
            CurrentZonePanel = new System.Windows.Forms.Panel();
            CurrentZoneLabel = new System.Windows.Forms.Label();
            ActiveZonesListGroup = new System.Windows.Forms.Panel();
            AddCustomZonePanel = new System.Windows.Forms.Panel();
            SettingsGroup = new System.Windows.Forms.GroupBox();
            TwitchConnectionGroup = new System.Windows.Forms.GroupBox();
            TwitchConnectionStatusLabel = new System.Windows.Forms.Label();
            TargetChannelLabel = new System.Windows.Forms.Label();
            LogGroupBox = new System.Windows.Forms.GroupBox();
            DiscordWebhookLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(SettingsLogSplitter)).BeginInit();
            SettingsLogSplitter.Panel1.SuspendLayout();
            SettingsLogSplitter.Panel2.SuspendLayout();
            SettingsLogSplitter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(ActiveZonesSettingsSplitter)).BeginInit();
            ActiveZonesSettingsSplitter.Panel1.SuspendLayout();
            ActiveZonesSettingsSplitter.Panel2.SuspendLayout();
            ActiveZonesSettingsSplitter.SuspendLayout();
            EnabledZonesBox.SuspendLayout();
            EnabledZonesContainer.SuspendLayout();
            CurrentZonePanel.SuspendLayout();
            ActiveZonesListGroup.SuspendLayout();
            AddCustomZonePanel.SuspendLayout();
            SettingsGroup.SuspendLayout();
            TwitchConnectionGroup.SuspendLayout();
            LogGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // SettingsLogSplitter
            // 
            SettingsLogSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            SettingsLogSplitter.Location = new System.Drawing.Point(0, 0);
            SettingsLogSplitter.Name = "SettingsLogSplitter";
            SettingsLogSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SettingsLogSplitter.Panel1
            // 
            SettingsLogSplitter.Panel1.Controls.Add(ActiveZonesSettingsSplitter);
            // 
            // SettingsLogSplitter.Panel2
            // 
            SettingsLogSplitter.Panel2.Controls.Add(LogGroupBox);
            SettingsLogSplitter.Size = new System.Drawing.Size(1013, 684);
            SettingsLogSplitter.SplitterDistance = 469;
            SettingsLogSplitter.TabIndex = 0;
            // 
            // ActiveZonesSettingsSplitter
            // 
            ActiveZonesSettingsSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            ActiveZonesSettingsSplitter.Location = new System.Drawing.Point(0, 0);
            ActiveZonesSettingsSplitter.Name = "ActiveZonesSettingsSplitter";
            // 
            // ActiveZonesSettingsSplitter.Panel1
            // 
            ActiveZonesSettingsSplitter.Panel1.Controls.Add(EnabledZonesBox);
            // 
            // ActiveZonesSettingsSplitter.Panel2
            // 
            ActiveZonesSettingsSplitter.Panel2.Controls.Add(SettingsGroup);
            ActiveZonesSettingsSplitter.Size = new System.Drawing.Size(1013, 469);
            ActiveZonesSettingsSplitter.SplitterDistance = 448;
            ActiveZonesSettingsSplitter.TabIndex = 0;
            // 
            // EnabledZonesBox
            // 
            EnabledZonesBox.Controls.Add(EnabledZonesContainer);
            EnabledZonesBox.Dock = System.Windows.Forms.DockStyle.Fill;
            EnabledZonesBox.Location = new System.Drawing.Point(0, 0);
            EnabledZonesBox.Name = "EnabledZonesBox";
            EnabledZonesBox.Size = new System.Drawing.Size(448, 469);
            EnabledZonesBox.TabIndex = 0;
            EnabledZonesBox.TabStop = false;
            EnabledZonesBox.Text = "Enabled Zones";
            // 
            // EnabledZonesContainer
            // 
            EnabledZonesContainer.Controls.Add(CurrentZonePanel);
            EnabledZonesContainer.Controls.Add(ActiveZonesListGroup);
            EnabledZonesContainer.Controls.Add(AddCustomZonePanel);
            EnabledZonesContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            EnabledZonesContainer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            EnabledZonesContainer.Location = new System.Drawing.Point(3, 16);
            EnabledZonesContainer.Name = "EnabledZonesContainer";
            EnabledZonesContainer.Size = new System.Drawing.Size(442, 450);
            EnabledZonesContainer.TabIndex = 0;
            EnabledZonesContainer.WrapContents = false;
            // 
            // CurrentZonePanel
            // 
            CurrentZonePanel.Controls.Add(this.addCurButton);
            CurrentZonePanel.Controls.Add(this.curZoneText);
            CurrentZonePanel.Controls.Add(CurrentZoneLabel);
            CurrentZonePanel.Location = new System.Drawing.Point(3, 3);
            CurrentZonePanel.Name = "CurrentZonePanel";
            CurrentZonePanel.Size = new System.Drawing.Size(436, 46);
            CurrentZonePanel.TabIndex = 0;
            // 
            // addCurButton
            // 
            this.addCurButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.addCurButton.Location = new System.Drawing.Point(366, 0);
            this.addCurButton.Name = "addCurButton";
            this.addCurButton.Size = new System.Drawing.Size(70, 46);
            this.addCurButton.TabIndex = 2;
            this.addCurButton.Text = "Add";
            this.addCurButton.UseVisualStyleBackColor = true;
            this.addCurButton.Click += new System.EventHandler(this.AddCurButton_Click);
            // 
            // curZoneText
            // 
            this.curZoneText.AutoSize = true;
            this.curZoneText.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.curZoneText.ForeColor = System.Drawing.Color.Fuchsia;
            this.curZoneText.Location = new System.Drawing.Point(82, 13);
            this.curZoneText.MaximumSize = new System.Drawing.Size(300, 0);
            this.curZoneText.Name = "curZoneText";
            this.curZoneText.Size = new System.Drawing.Size(71, 21);
            this.curZoneText.TabIndex = 1;
            this.curZoneText.Text = "Unknown";
            this.curZoneText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CurrentZoneLabel
            // 
            CurrentZoneLabel.AutoSize = true;
            CurrentZoneLabel.Location = new System.Drawing.Point(4, 19);
            CurrentZoneLabel.Name = "CurrentZoneLabel";
            CurrentZoneLabel.Size = new System.Drawing.Size(72, 13);
            CurrentZoneLabel.TabIndex = 0;
            CurrentZoneLabel.Text = "Current Zone:";
            // 
            // ActiveZonesListGroup
            // 
            ActiveZonesListGroup.Controls.Add(this.removeButton);
            ActiveZonesListGroup.Controls.Add(this.activeZonesList);
            ActiveZonesListGroup.Location = new System.Drawing.Point(3, 55);
            ActiveZonesListGroup.Name = "ActiveZonesListGroup";
            ActiveZonesListGroup.Size = new System.Drawing.Size(436, 343);
            ActiveZonesListGroup.TabIndex = 1;
            // 
            // removeButton
            // 
            this.removeButton.Location = new System.Drawing.Point(366, 156);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(67, 40);
            this.removeButton.TabIndex = 1;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // activeZonesList
            // 
            this.activeZonesList.Dock = System.Windows.Forms.DockStyle.Left;
            this.activeZonesList.FormattingEnabled = true;
            this.activeZonesList.HorizontalScrollbar = true;
            this.activeZonesList.Items.AddRange(new object[] {
            "Zone 1",
            "Zone 2"});
            this.activeZonesList.Location = new System.Drawing.Point(0, 0);
            this.activeZonesList.Name = "activeZonesList";
            this.activeZonesList.Size = new System.Drawing.Size(360, 343);
            this.activeZonesList.TabIndex = 0;
            // 
            // AddCustomZonePanel
            // 
            AddCustomZonePanel.Controls.Add(this.addCustomZoneButton);
            AddCustomZonePanel.Controls.Add(this.addZoneTextBox);
            AddCustomZonePanel.Location = new System.Drawing.Point(3, 404);
            AddCustomZonePanel.Name = "AddCustomZonePanel";
            AddCustomZonePanel.Size = new System.Drawing.Size(433, 29);
            AddCustomZonePanel.TabIndex = 2;
            // 
            // addCustomZoneButton
            // 
            this.addCustomZoneButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.addCustomZoneButton.Location = new System.Drawing.Point(366, 0);
            this.addCustomZoneButton.Name = "addCustomZoneButton";
            this.addCustomZoneButton.Size = new System.Drawing.Size(67, 29);
            this.addCustomZoneButton.TabIndex = 1;
            this.addCustomZoneButton.Text = "Add";
            this.addCustomZoneButton.UseVisualStyleBackColor = true;
            this.addCustomZoneButton.Click += new System.EventHandler(this.addCustomZoneButton_Click);
            // 
            // addZoneTextBox
            // 
            this.addZoneTextBox.Location = new System.Drawing.Point(0, 4);
            this.addZoneTextBox.Name = "addZoneTextBox";
            this.addZoneTextBox.Size = new System.Drawing.Size(360, 20);
            this.addZoneTextBox.TabIndex = 0;
            // 
            // SettingsGroup
            // 
            SettingsGroup.Controls.Add(this.discordWebhookTextBox);
            SettingsGroup.Controls.Add(DiscordWebhookLabel);
            SettingsGroup.Controls.Add(TwitchConnectionGroup);
            SettingsGroup.Controls.Add(this.targetChannelTextBox);
            SettingsGroup.Controls.Add(TargetChannelLabel);
            SettingsGroup.Controls.Add(this.saveSettingsButton);
            SettingsGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            SettingsGroup.Location = new System.Drawing.Point(0, 0);
            SettingsGroup.Name = "SettingsGroup";
            SettingsGroup.Size = new System.Drawing.Size(561, 469);
            SettingsGroup.TabIndex = 0;
            SettingsGroup.TabStop = false;
            SettingsGroup.Text = "Settings";
            // 
            // TwitchConnectionGroup
            // 
            TwitchConnectionGroup.Controls.Add(this.twitchAccountLinkButton);
            TwitchConnectionGroup.Controls.Add(this.connectionStatusText);
            TwitchConnectionGroup.Controls.Add(TwitchConnectionStatusLabel);
            TwitchConnectionGroup.Location = new System.Drawing.Point(10, 20);
            TwitchConnectionGroup.Name = "TwitchConnectionGroup";
            TwitchConnectionGroup.Size = new System.Drawing.Size(526, 113);
            TwitchConnectionGroup.TabIndex = 6;
            TwitchConnectionGroup.TabStop = false;
            TwitchConnectionGroup.Text = "Twitch Connection";
            // 
            // twitchAccountLinkButton
            // 
            this.twitchAccountLinkButton.Location = new System.Drawing.Point(386, 35);
            this.twitchAccountLinkButton.Name = "twitchAccountLinkButton";
            this.twitchAccountLinkButton.Size = new System.Drawing.Size(109, 52);
            this.twitchAccountLinkButton.TabIndex = 2;
            this.twitchAccountLinkButton.Text = "Link Account";
            this.twitchAccountLinkButton.UseVisualStyleBackColor = true;
            this.twitchAccountLinkButton.Click += new System.EventHandler(this.twitchAccountLinkButton_Click);
            // 
            // connectionStatusText
            // 
            this.connectionStatusText.AutoSize = true;
            this.connectionStatusText.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.connectionStatusText.Location = new System.Drawing.Point(54, 48);
            this.connectionStatusText.Name = "connectionStatusText";
            this.connectionStatusText.Size = new System.Drawing.Size(193, 25);
            this.connectionStatusText.TabIndex = 1;
            this.connectionStatusText.Text = "No Account Linked";
            this.connectionStatusText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TwitchConnectionStatusLabel
            // 
            TwitchConnectionStatusLabel.AutoSize = true;
            TwitchConnectionStatusLabel.Location = new System.Drawing.Point(99, 35);
            TwitchConnectionStatusLabel.Name = "TwitchConnectionStatusLabel";
            TwitchConnectionStatusLabel.Size = new System.Drawing.Size(95, 13);
            TwitchConnectionStatusLabel.TabIndex = 0;
            TwitchConnectionStatusLabel.Text = "Connection status:";
            // 
            // targetChannelTextBox
            // 
            this.targetChannelTextBox.Location = new System.Drawing.Point(112, 184);
            this.targetChannelTextBox.Name = "targetChannelTextBox";
            this.targetChannelTextBox.Size = new System.Drawing.Size(337, 20);
            this.targetChannelTextBox.TabIndex = 4;
            // 
            // TargetChannelLabel
            // 
            TargetChannelLabel.AutoSize = true;
            TargetChannelLabel.Location = new System.Drawing.Point(7, 187);
            TargetChannelLabel.Name = "TargetChannelLabel";
            TargetChannelLabel.Size = new System.Drawing.Size(83, 13);
            TargetChannelLabel.TabIndex = 3;
            TargetChannelLabel.Text = "Target Channel:";
            // 
            // saveSettingsButton
            // 
            this.saveSettingsButton.Location = new System.Drawing.Point(451, 440);
            this.saveSettingsButton.Name = "saveSettingsButton";
            this.saveSettingsButton.Size = new System.Drawing.Size(104, 23);
            this.saveSettingsButton.TabIndex = 2;
            this.saveSettingsButton.Text = "Save Settings";
            this.saveSettingsButton.UseVisualStyleBackColor = true;
            this.saveSettingsButton.Click += new System.EventHandler(this.saveSettingsButton_Click);
            // 
            // LogGroupBox
            // 
            LogGroupBox.Controls.Add(this.logRTBox);
            LogGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            LogGroupBox.Location = new System.Drawing.Point(0, 0);
            LogGroupBox.Name = "LogGroupBox";
            LogGroupBox.Size = new System.Drawing.Size(1013, 211);
            LogGroupBox.TabIndex = 0;
            LogGroupBox.TabStop = false;
            LogGroupBox.Text = "Log";
            // 
            // logRTBox
            // 
            this.logRTBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logRTBox.Location = new System.Drawing.Point(3, 16);
            this.logRTBox.Name = "logRTBox";
            this.logRTBox.ReadOnly = true;
            this.logRTBox.Size = new System.Drawing.Size(1007, 192);
            this.logRTBox.TabIndex = 0;
            this.logRTBox.Text = "";
            // 
            // DiscordWebhookLabel
            // 
            DiscordWebhookLabel.AutoSize = true;
            DiscordWebhookLabel.Location = new System.Drawing.Point(7, 213);
            DiscordWebhookLabel.Name = "DiscordWebhookLabel";
            DiscordWebhookLabel.Size = new System.Drawing.Size(96, 13);
            DiscordWebhookLabel.TabIndex = 7;
            DiscordWebhookLabel.Text = "Discord Webhook:";
            // 
            // discordWebhookTextBox
            // 
            this.discordWebhookTextBox.Location = new System.Drawing.Point(112, 210);
            this.discordWebhookTextBox.Name = "discordWebhookTextBox";
            this.discordWebhookTextBox.Size = new System.Drawing.Size(337, 20);
            this.discordWebhookTextBox.TabIndex = 8;
            // 
            // ClippyUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(SettingsLogSplitter);
            this.Name = "ClippyUI";
            this.Size = new System.Drawing.Size(1013, 684);
            SettingsLogSplitter.Panel1.ResumeLayout(false);
            SettingsLogSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(SettingsLogSplitter)).EndInit();
            SettingsLogSplitter.ResumeLayout(false);
            ActiveZonesSettingsSplitter.Panel1.ResumeLayout(false);
            ActiveZonesSettingsSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(ActiveZonesSettingsSplitter)).EndInit();
            ActiveZonesSettingsSplitter.ResumeLayout(false);
            EnabledZonesBox.ResumeLayout(false);
            EnabledZonesContainer.ResumeLayout(false);
            CurrentZonePanel.ResumeLayout(false);
            CurrentZonePanel.PerformLayout();
            ActiveZonesListGroup.ResumeLayout(false);
            AddCustomZonePanel.ResumeLayout(false);
            AddCustomZonePanel.PerformLayout();
            SettingsGroup.ResumeLayout(false);
            SettingsGroup.PerformLayout();
            TwitchConnectionGroup.ResumeLayout(false);
            TwitchConnectionGroup.PerformLayout();
            LogGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		#endregion
		public ClippyUI()
		{
			InitializeComponent();
		}

        //Form controls
        private Button addCurButton;
        private Label curZoneText;
        private Button removeButton;
        private ListBox activeZonesList;
        private Button addCustomZoneButton;
        private TextBox addZoneTextBox;
        public RichTextBox logRTBox;
        Label lblStatus;    // The status label that appears in ACT's Plugin tab
        private Button saveSettingsButton;
        private TextBox targetChannelTextBox;
        private Button twitchAccountLinkButton;
        private Label connectionStatusText;

        //Callbacks to update program state
        private AddZoneDel addZone;
        private RmZoneDel rmZone;
        private UpdateSettingsDel updateSettings;
        private StartTwitchAuthDel auth;
        private CreateClipDel clip;
        private TextBox discordWebhookTextBox;
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public void InitUI(TabPage pluginScreenSpace, Label pluginStatusText)
		{
			lblStatus = pluginStatusText;   // Hand the status label's reference to our local var
			pluginScreenSpace.Controls.Add(this);   // Add this UserControl to the tab ACT provides
			this.Dock = DockStyle.Fill; // Expand the UserControl to fill the tab's client space
			// Create some sort of parsing event handler.  After the "+=" hit TAB twice and the code will be generated for you.

			lblStatus.Text = "Plugin Started";
		}

        public void SetDelegates(AddZoneDel addZone, RmZoneDel rmZone, UpdateSettingsDel updateSettings, StartTwitchAuthDel auth, CreateClipDel clip) {
            this.addZone = addZone;
            this.rmZone = rmZone;
            this.updateSettings = updateSettings;
            this.auth = auth;
            this.clip = clip;
        }

        public void UpdateZones(string[] activeZones) {
            Logger.Debug("Updating zones list in UI");
            Logger.Trace("New zones list: {activeZones}");
            //Update zones list
            if(ActGlobals.oFormActMain.InvokeRequired) {
                ActGlobals.oFormActMain.Invoke(new Action(() => {
                    this.activeZonesList.BeginUpdate();
                    //Remove all existing items
                    this.activeZonesList.Items.Clear();
                    //Insert all new active zones
                    this.activeZonesList.Items.AddRange(activeZones);
                    this.activeZonesList.EndUpdate();
                }));
            } else {
                this.activeZonesList.BeginUpdate();
                //Remove all existing items
                this.activeZonesList.Items.Clear();
                //Insert all new active zones
                this.activeZonesList.Items.AddRange(activeZones);
                this.activeZonesList.EndUpdate();
            }
        }

        public void UpdateCurrentZone(string zoneName) {
            Logger.Debug("Updating current zone in UI");
            Logger.Trace("New zone: {zoneName}");
            ThreadInvokes.ControlSetText(ActGlobals.oFormActMain, curZoneText, zoneName);
        }

        public void UpdateTwitchConnectionStatus(string text) {
            Logger.Debug("Updating current Twitch account status in UI");
            ThreadInvokes.ControlSetText(ActGlobals.oFormActMain, connectionStatusText, text);
        }

        public void UpdateTwitchChannel(string channel) {
            Logger.Debug("Updating target Twitch channel in UI");
            Logger.Trace("New channel: {channel}");
            ThreadInvokes.ControlSetText(ActGlobals.oFormActMain, targetChannelTextBox, channel);
        }

        public void UpdateDiscordWebhook(string webhook) {
            Logger.Debug("Updating discord webhook in UI");
            Logger.Trace("New zone: {zoneName}");
            ThreadInvokes.ControlSetText(ActGlobals.oFormActMain, discordWebhookTextBox, webhook);
        }

		public void DeInitUI()
		{
			lblStatus.Text = "Plugin Exited";
		}

        private void AddCurButton_Click(object sender, EventArgs e) {
            string zone = this.curZoneText.Text;
            this.addZone.Invoke(zone);
        }

        private void removeButton_Click(object sender, EventArgs e) {
            string zone = (string)this.activeZonesList.SelectedItem;
            this.rmZone.Invoke(zone);
        }

        private void addCustomZoneButton_Click(object sender, EventArgs e) {
            string zone = this.addZoneTextBox.Text;
            this.addZone.Invoke(zone);
            this.addZoneTextBox.Text = "";
        }

        private void saveSettingsButton_Click(object sender, EventArgs e) {
            string channel = this.targetChannelTextBox.Text;
            string webhook = this.discordWebhookTextBox.Text;
            this.updateSettings.Invoke(channel, webhook);
        }

        private async void twitchAccountLinkButton_Click(object sender, EventArgs e) {
            await this.auth();
        }
    }
}
