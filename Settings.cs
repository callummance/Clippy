using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NLog;

namespace Clippy {
    [JsonObject (MemberSerialization = MemberSerialization.OptIn)]
    public class Settings {
        private string file;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        //Settings
        [JsonProperty]
        public string TwitchAuthToken { get; set; }

        [JsonProperty]
        public string TwitchChannel { get; set; }

        [JsonProperty]
        public float TwitchDelaySecs { get; set; }

        [JsonProperty]
        public string DiscordWebhookUrl{ get; set; }

        private readonly object _ActiveZonesLock = new object();
        [JsonProperty]
        private HashSet<string> ActiveZones = new HashSet<string>();

        //Private constructor for JSON deserialization
        [JsonConstructor]
        private Settings(string twitchAuthToken, string twitchChannel, float twitchDelaySecs, HashSet<string> activeZones) {
            this.TwitchAuthToken= twitchAuthToken;
            this.TwitchChannel = twitchChannel;
            this.TwitchDelaySecs = twitchDelaySecs;
            this.ActiveZones = activeZones;
        }

        //Create new empty settings object
        private Settings(string file) { }

        //Load a new settings object from a saved json file
        public static Settings LoadSettings(string file) {
            Settings res;
            try {
                string jsonStr = File.ReadAllText(file);
                res = JsonConvert.DeserializeObject<Settings>(jsonStr);
            } catch (FileNotFoundException) {
                logger.Info($"Couldn't find settings file at {file}, creating blank settings object...");
                res = new Settings(file);
                res.SaveSettings();
            } catch (JsonException e) {
                logger.Error(e, "Encountered unexpected error whilst loading settings");
                res = new Settings(file);
            }
            res.file = file;
            return res;
        }

        //Save contents of settings to disk in JSON format
        public void SaveSettings() {
            string serString = JsonConvert.SerializeObject(this);
            File.WriteAllText(this.file, serString);
        }

        public void InsertZone(string zoneName) {
            lock(_ActiveZonesLock) {
                this.ActiveZones.Add(zoneName);
            }
        }

        public void RemoveZone(string zoneName) {
            lock(_ActiveZonesLock) {
                this.ActiveZones.Remove(zoneName);
            }
        }

        public bool IsActiveZone(string zoneName) {
            lock(_ActiveZonesLock) {
                return this.ActiveZones.Contains(zoneName);
            }
        }

        public string[] GetZonesArr() {
            lock(_ActiveZonesLock) {
                string[] res = new string[this.ActiveZones.Count];
                this.ActiveZones.CopyTo(res);
                return res;
            }
        }


    }
}
