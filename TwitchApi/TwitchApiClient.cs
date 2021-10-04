using Newtonsoft.Json;
using NLog;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Clippy.TwitchApi {
    class TwitchApiClient {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        const string TWITCH_API_BASE = "https://api.twitch.tv/helix";

        private ITwitchApiAuth auth;

        public TwitchApiClient(ITwitchApiAuth auth) {
            this.auth = auth;
        }

        public async Task<ClipCreateResponse> CreateClip(string broadcastId, bool hasDelay) {
            string url = $"{TWITCH_API_BASE}/clips?broadcaster_id={broadcastId}&has_delay={hasDelay}";
            try {
                HttpResponseMessage resp = await auth.MakeReq(url, HttpMethod.Post);
                HttpResponseHeaders headers = resp.Headers;
                string respBody = await resp.Content.ReadAsStringAsync();
                logger.Trace($"returned body: {respBody}");
                resp.EnsureSuccessStatusCode();
                ClipCreateResponse res = JsonConvert.DeserializeObject<ClipCreateResponse>(respBody);

                //Save ratelimiting headers
                string limit = headers.GetValues("Ratelimit-Helixclipscreation-Limit").ToString();
                string remaining = headers.GetValues("Ratelimit-Helixclipscreation-Remaining").ToString();

                logger.Info($"Clip creation rate limit: {remaining} remaining of {limit} total.");

                return res;
            } catch (HttpRequestException e) {
                logger.Warn(e, $"Failed to create clip due to error {e}.");
                logger.Debug($"query url: {url}");
                throw e;
            }
        }

        private async Task<GetStreamsResponse> GetStreamsPage(GetStreamsOpts opts) {
            string url = $"{TWITCH_API_BASE}/streams{UrlParamEncoder.UrlParamEncoder.ToUrlParams(opts)}";
            try {
                HttpResponseMessage resp = await auth.MakeReq(url, HttpMethod.Get);
                string respBody = await resp.Content.ReadAsStringAsync();
                logger.Trace($"returned body: {respBody}");
                resp.EnsureSuccessStatusCode();
                GetStreamsResponse res = JsonConvert.DeserializeObject<GetStreamsResponse>(respBody);

                return res;
            } catch (HttpRequestException e) {
                logger.Warn(e, "Failed to fetch streams details due to error.");
                logger.Debug($"query url: {url}");
                throw e;
            }
        }

        public async Task<List<StreamData>> GetStreams(GetStreamsOpts opts) {
            List<StreamData> streams = new List<StreamData>();

            for (; ;) {
                GetStreamsResponse pg = await this.GetStreamsPage(opts);
                streams.AddRange(pg.Data);
                if (pg.Pagination == null || pg.Pagination.Cursor == null || pg.Pagination.Cursor == "") {
                    //No more pages
                    return streams;
                } else {
                    //We have at least one more page so update opts and continue loop
                    opts.After = pg.Pagination.Cursor;
                }
            }
        }
    }
}
