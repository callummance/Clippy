using Newtonsoft.Json;

namespace Clippy.DiscordWebhooks {
    class WebhookEmbed {
        [JsonProperty(PropertyName = "author")]
        public EmbedAuthor Author{ get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title{ get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url{ get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description{ get; set; }

        [JsonProperty(PropertyName = "fields")]
        public EmbedField[] Fields{ get; set; }

        [JsonProperty(PropertyName = "thumbnail")]
        public EmbedImage Thumbnail{ get; set; }

        [JsonProperty(PropertyName = "image")]
        public EmbedImage Image{ get; set; }

        [JsonProperty(PropertyName = "footer")]
        public EmbedFooter Footer{ get; set; }

        [JsonProperty(PropertyName = "timestamp")]
        public string Timestamp{ get; set; }

        [JsonProperty(PropertyName = "color")]
        public int Colour{ get; set; }
    }
}
