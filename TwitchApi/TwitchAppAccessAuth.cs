using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;

namespace Clippy.TwitchApi {
    class TwitchAppAccessAuth : ITwitchApiAuth {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        const int MAX_401_RETRIES = 1;
        const string TWITCH_OAUTH2_TOKEN_URL = "https://id.twitch.tv/oauth2/token";


        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string scopes;
        private OauthTokenResponse lastTokenResponse;

        static readonly HttpClient hClient = new HttpClient();


        public TwitchAppAccessAuth(string clientId, string clientSecret, string scopes) {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.scopes = scopes;

            var task = this.GetNewToken();
            task.Wait();
        }

        public async Task<HttpResponseMessage> MakeReq(string url, HttpMethod method) {
            string tok = await this.GetToken();
            HttpRequestMessage req = new HttpRequestMessage() {
                RequestUri = new Uri(url),
                Method = method,
            };
            req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tok);
            req.Headers.Add("Client-Id", this.clientId);

            for (int retries = 0; retries <= MAX_401_RETRIES; retries++) {
                HttpResponseMessage resp = await hClient.SendAsync(req);
                string respStr = await resp.Content.ReadAsStringAsync();
                logger.Trace($"Got response {respStr}");
                if (resp.IsSuccessStatusCode) {
                    //Successful response
                    return resp;
                } else if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
                    //Authorization broke, so try getting a new token
                    await this.GetNewToken();
                } else {
                    //Bad response, let caller check and deal with it
                    return resp;
                }
            }
            return null;
        }

        private async Task<string> GetToken() {
            long time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            if (lastTokenResponse == null) {
                //We just created the client so need to fetch a token
                logger.Debug("Fetching new twitch oauth token as we do not have one.");
                await this.GetNewToken();
                return this.lastTokenResponse.AccessToken;
            } else if (lastTokenResponse.ExpiresIn + lastTokenResponse.Timestamp <= time) {
                //Token expired, so get a new one
                logger.Debug("Fetching new twitch oauth token as the existing one expired.");
                await this.GetNewToken();
                return this.lastTokenResponse.AccessToken;
            } else {
                //We have a valid token, so just return it
                logger.Debug("Using existing twitch oauth token.");
                return this.lastTokenResponse.AccessToken;
            }
        }


        private async Task GetNewToken() {
            OauthTokenReq authOpts = new OauthTokenReq();
            authOpts.ClientId = clientId;
            authOpts.ClientSecret = clientSecret;
            authOpts.GrantType = "client_credentials";
            authOpts.Scopes = scopes;

            try {
                string url = TWITCH_OAUTH2_TOKEN_URL + UrlParamEncoder.UrlParamEncoder.ToUrlParams(authOpts);
                HttpRequestMessage req = new HttpRequestMessage() {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Post,
                };

                HttpResponseMessage resp = await hClient.SendAsync(req);
                resp.EnsureSuccessStatusCode();
                string respBody = await resp.Content.ReadAsStringAsync();
                OauthTokenResponse res = JsonConvert.DeserializeObject<OauthTokenResponse>(respBody);
                res.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                this.lastTokenResponse = res;
            } catch(HttpRequestException e) {
                logger.Error(e, "Failed to fetch Twitch API token due to error.");
                throw e;
            }
        }



    }
}
