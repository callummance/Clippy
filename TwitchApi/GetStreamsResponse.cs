using Newtonsoft.Json;

namespace Clippy.TwitchApi {
    public class GetStreamsResponse {
        [JsonProperty(PropertyName = "data")]
        public StreamData[] Data { get; set; }

        [JsonProperty(PropertyName = "pagination")]
        public Pagination Pagination { get; set; }
    }
}
