using Newtonsoft.Json;

namespace Clippy.TwitchApi {
    public class StreamData {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "user_login")]
        public string UserLogin { get; set; }

        [JsonProperty(PropertyName = "user_name")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "game_id")]
        public string GameId { get; set; }

        [JsonProperty(PropertyName = "game_name")]
        public string GameName { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "viewer_count")]
        public int ViewerCount { get; set; }

        [JsonProperty(PropertyName = "started_at")]
        public string StartedAt { get; set; }

        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }

        [JsonProperty(PropertyName = "thumbnail_url")]
        public string ThumbnailUrl { get; set; }

        [JsonProperty(PropertyName = "tag_ids")]
        public string[] TagIds { get; set; }

        [JsonProperty(PropertyName = "is_mature")]
        public bool IsMature { get; set; }
    }
}
