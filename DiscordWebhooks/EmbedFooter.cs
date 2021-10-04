using Newtonsoft.Json;

namespace Clippy.DiscordWebhooks {
    class EmbedFooter {
        [JsonProperty(PropertyName = "text")]
        public string Text{ get; set; }

        [JsonProperty(PropertyName = "icon_url")]
        public string IconUrl{ get; set; }
    }
}
