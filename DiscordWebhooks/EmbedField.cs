using Newtonsoft.Json;

namespace Clippy.DiscordWebhooks {
    class EmbedField{
        [JsonProperty(PropertyName = "name")]
        public string Name{ get; set; }

        [JsonProperty(PropertyName = "value")]
        public string value{ get; set; }

        [JsonProperty(PropertyName = "inline")]
        public bool Inline{ get; set; }
    }
}
