using Newtonsoft.Json;

namespace Clippy.DiscordWebhooks {
    class EmbedAuthor {
        [JsonProperty(PropertyName = "name")]
        public string Name{ get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url{ get; set; }

        [JsonProperty(PropertyName = "icon_url")]
        public string IconUrl{ get; set; }
    }
}
