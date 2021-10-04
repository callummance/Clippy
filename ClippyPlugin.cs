using Advanced_Combat_Tracker;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System;

namespace Clippy {
    public class ClippyPlugin : IActPluginV1 {
        private const string AppId = "989yjktzodbzb72lc523pin4gq3eoo";

        private ClippyUI ui;
        private volatile Settings settings;
        string settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config\\Clippy.config.json");

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private static Regex _wipeRegex = new Regex(@"^.{14} 21:[^:]+:40000010");
        private static Regex _zoneChangeRegex = new Regex(@"^.{14} 01:Changed Zone to (.*)");

        private bool _zoneIsKnown = false;

        private TwitchApi.TwitchImplicitCodeAuth auth;
        private TwitchClipper clipper;

        public ClippyPlugin() {
            this.ui = new ClippyUI();
        }

        public void DeInitPlugin() {
            this.settings.SaveSettings();
            ui.DeInitUI();
        }

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText) {
            this.settings = Settings.LoadSettings(this.settingsFile);
            ui.InitUI(pluginScreenSpace, pluginStatusText);

            InitLog();
            //Create clipper
            if (this.settings.TwitchAuthToken != null && this.settings.TwitchAuthToken != "") {
                this.auth = new TwitchApi.TwitchImplicitCodeAuth(this.settings.TwitchAuthToken, AppId, "clips:edit");
            } else {
                this.auth = new TwitchApi.TwitchImplicitCodeAuth(AppId, "clips:edit");
            }
            this.clipper = new TwitchClipper(auth);
            //Setup UI
            ui.SetDelegates(this.AddZone, this.RemoveZone, this.UpdateSettings, this.TwitchAuth, this.clipper.NewClipAsync);
            ui.UpdateZones(this.settings.GetZonesArr());
            this.UpdateTwitchConnectionStatus();
            ui.UpdateTwitchChannel(this.settings.TwitchChannel);


            //Register handlers
            ActGlobals.oFormActMain.OnLogLineRead += new LogLineEventDelegate(oFormActMain_OnLogLineRead);
        }

        private void InitLog() {
            var config = new NLog.Config.LoggingConfiguration();

            //Log to forms textbox
            var logFormTgt = new NLog.Windows.Forms.RichTextBoxTarget();
            logFormTgt.FormName = "FormACTMain";
            logFormTgt.ControlName = "logRTBox";
            logFormTgt.AllowAccessoryFormCreation = false;
            logFormTgt.MessageRetention = NLog.Windows.Forms.RichTextBoxTargetMessageRetentionStrategy.OnlyMissed;
            logFormTgt.AutoScroll = true;
            logFormTgt.UseDefaultRowColoringRules = true;


            config.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Error, logFormTgt);
            NLog.LogManager.Configuration = config;
            NLog.LogManager.ReconfigExistingLoggers();
        }

        private void oFormActMain_OnLogLineRead(bool isImport, LogLineEventArgs actionInfo) {
            string zone = actionInfo.detectedZone;
            string line = actionInfo.logLine;

            Logger.Trace(line);

            //If we don't already know what zone we are in, set it
            if (!this._zoneIsKnown) {
                this.ui.UpdateCurrentZone(zone);
                this._zoneIsKnown = true;
            }

            //Check for wipes
            if (_wipeRegex.IsMatch(line) && settings.IsActiveZone(zone)) {
                //Line indicates a wipe and we are currently in an active zone
                Logger.Debug("Found log line indicating a wipe in a selected zone: {line}");
                Logger.Info("Creating twitch clip of wipe in zone {zone}");

                string curZone = ActGlobals.oFormActMain.ActiveZone.ZoneName;
                if (this.settings.IsActiveZone(curZone)) {
                    //We had a wipe in an active zone, so make a clip
                    Task.Run(async () => {
                        try {
                            TwitchApi.ClipCreateResponse resp = await this.clipper.NewClipAsync(this.settings.TwitchChannel);
                            await this.PublishClipLink(resp.EditUrl);
                        } catch (Exception e){
                            Logger.Error(e, $"Failed to create clip due to error {e}");
                        }
                    });
                }
            }

            //Check for zone change
            if (_zoneChangeRegex.IsMatch(line)) {
                Logger.Debug("Found log line indicating a zone change: {line}");
                this.ui.UpdateCurrentZone(zone);
            }
        }

        private async Task PublishClipLink(string link) {
            //Dump in log
            Logger.Info($"Created clip at edit url {link}");
            //Publish to discord webhook
            DiscordWebhooks.WebhookEmbed embed = new DiscordWebhooks.WebhookEmbed() {
                Author = new DiscordWebhooks.EmbedAuthor() {
                    Name = "Clippy",
                    IconUrl = "https://www.pngfind.com/pngs/m/287-2872829_transparent-paperclip-gif-clippy-the-paperclip-gif-hd.png",
                    Url = "https://github.com/callummance/Clippy",
                },
                Title = $"Twitch clip of wipe in {ActGlobals.oFormActMain.ActiveZone.ZoneName}",
                Url = link,
                Description = "Click on the link to open clip editor",
                Timestamp = DateTime.Now.ToString("O", System.Globalization.CultureInfo.InvariantCulture),
                Colour = 0x990083,
            };
            DiscordWebhooks.WebhookMessage msg = new DiscordWebhooks.WebhookMessage() {
                Username = "Clippy",
                Embeds = new DiscordWebhooks.WebhookEmbed[] {embed},
                AvatarUrl = "https://www.pngfind.com/pngs/m/287-2872829_transparent-paperclip-gif-clippy-the-paperclip-gif-hd.png",
            };

            try {
                await msg.Submit(this.settings.DiscordWebhookUrl);
            } catch (Exception e) {
                Logger.Warn(e, $"Failed to submit edit link to discord webhook due to error {e}");
            }
        }


        //Updates the clipping settings in the settings file and updates the UI to reflect it
        public void UpdateSettings(string twitchChannel, string discordWebhook) {
            //Update Channel
            if (this.settings.TwitchChannel != twitchChannel) {
                this.settings.TwitchChannel = twitchChannel;
                this.ui.UpdateTwitchChannel(twitchChannel);
            }

            //Update webhook
            if (this.settings.DiscordWebhookUrl != discordWebhook) {
                this.settings.DiscordWebhookUrl = discordWebhook;
                this.ui.UpdateDiscordWebhook(discordWebhook);
            }

            //Save to disk
            this.settings.SaveSettings();
        }

        //Adds a zone to the settings file and updates the zones list on the UI
        public void AddZone(string zoneName) {
            this.settings.InsertZone(zoneName);
            this.ui.UpdateZones(this.settings.GetZonesArr());
            //Save to disk
            this.settings.SaveSettings();
        }

        //Removes a zone from the active zones list in the settings file and updates the UI accordingly
        public void RemoveZone(string zoneName) {
            this.settings.RemoveZone(zoneName);
            this.ui.UpdateZones(this.settings.GetZonesArr());
            //Save to disk
            this.settings.SaveSettings();
        }

        public async Task TwitchAuth() {
            string newTok = await this.auth.RequestUserAuth();
            //Update Settings
            if (this.settings.TwitchAuthToken != newTok) {
                this.settings.TwitchAuthToken = newTok;
                this.UpdateTwitchConnectionStatus();
            }

            //Save to disk
            this.settings.SaveSettings();
        }

        private void UpdateTwitchConnectionStatus() {
            if (this.auth.HasAuthCode()) {
                this.ui.UpdateTwitchConnectionStatus("Twitch account linked");
            } else { 
                this.ui.UpdateTwitchConnectionStatus("No Account Linked");
            }
        }
    }


}
