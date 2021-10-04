using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Clippy.DiscordWebhooks {
    [JsonObject (MemberSerialization = MemberSerialization.OptIn)]
    class WebhookMessage {
        private static HttpClient hClient = new HttpClient();

        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonProperty(PropertyName = "content")]
        public string Content{ get; set; }

        [JsonProperty(PropertyName = "embeds")]
        public WebhookEmbed[] Embeds{ get; set; }

        public async Task Submit(string whUrl) {
            string payload = JsonConvert.SerializeObject(this);
            HttpRequestMessage req = new HttpRequestMessage() {
                RequestUri = new Uri(whUrl),
                Method = HttpMethod.Post,
            };
            req.Headers.Add("Content-type", "application/json");

            HttpResponseMessage resp = await hClient.SendAsync(req);
            string respStr = await resp.Content.ReadAsStringAsync();
        }
    }
}
