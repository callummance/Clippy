using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Clippy.TwitchApi{
    public class ClipCreateResponse {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "edit_url")]
        public string EditUrl { get; set; }
    }
}
