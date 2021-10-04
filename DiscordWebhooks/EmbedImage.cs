using Newtonsoft.Json;

namespace Clippy.DiscordWebhooks {
    class EmbedImage{
        [JsonProperty(PropertyName = "url")]
        public string Url{ get; set; }
    }
}
