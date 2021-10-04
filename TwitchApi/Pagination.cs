using Newtonsoft.Json;

namespace Clippy.TwitchApi {
    public class Pagination {
        [JsonProperty(PropertyName = "cursor")]
        public string Cursor { get; set; }
    }
}
