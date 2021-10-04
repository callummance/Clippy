using NLog;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace Clippy.TwitchApi {

    class TwitchImplicitCodeAuth : ITwitchApiAuth {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private HttpClient hClient = new HttpClient();

        const string TWITCH_OAUTH2_AUTH_URL = "https://id.twitch.tv/oauth2/authorize";
        const int REDIRECT_PORT = 6942;

        private readonly string clientId;
        private readonly string scopes;

        private string authCode;

        public TwitchImplicitCodeAuth(string clientId, string scopes) {
            this.clientId = clientId;
            this.scopes = scopes;
        }

        public TwitchImplicitCodeAuth(string authCode, string clientId, string scopes) {
            this.authCode = authCode;
            this.clientId = clientId;
            this.scopes = scopes;
        }

        public async Task<HttpResponseMessage> MakeReq(string url, HttpMethod method) {
            HttpRequestMessage req = new HttpRequestMessage() {
                RequestUri = new Uri(url),
                Method = method,
            };
            req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this.authCode);
            req.Headers.Add("Client-Id", this.clientId);

            HttpResponseMessage resp = await hClient.SendAsync(req);
            string respStr = await resp.Content.ReadAsStringAsync();
            logger.Trace($"Got response {respStr}");
            if (resp.IsSuccessStatusCode) {
                //Successful response
                return resp;
            } else if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
                //Authorization broke, so write to log and return null
                logger.Error("Authorization failed when making request to the twitch API. You may need to relink yout Twitch account.");
                return null;
            } else {
                //Bad response, let caller check and deal with it
                return resp;
            }
        }

        public bool HasAuthCode() {
            return authCode != null && authCode != "";
        }

        public async Task<string> RequestUserAuth() {
            //Build request params
            string redirect = $"http://localhost:{REDIRECT_PORT}";
            OauthTokenReq req = new OauthTokenReq() {
                ClientId = this.clientId,
                RedirectUri = $"{redirect}/implicit/",
                ResponseType = "token",
                Scopes = this.scopes,
            };
            string url = $"{TWITCH_OAUTH2_AUTH_URL}{UrlParamEncoder.UrlParamEncoder.ToUrlParams(req)}";

            //Try to start listener on random port and send user to twitch
            try {
                Task<string> t = this.ListenForCallback(redirect);
                this.OpenBrowser(url);
                string res = await t;

                this.authCode = res;
                return res;
            } catch (Exception e) {
                logger.Warn(e, $"Failed to start http listener due to error {e}");
                return null;
            }
        }


        private async Task<string> ListenForCallback(string uri) {
            HttpListener listener = new HttpListener();
            try {

                listener.Prefixes.Add($"{uri}/implicit/");
                listener.Prefixes.Add($"{uri}/submitcode/");
                listener.Start();

                for (; ; ) {
                    //Handle request when it comes through
                    HttpListenerContext c = await listener.GetContextAsync();
                    HttpListenerRequest req = c.Request;
                    HttpListenerResponse resp = c.Response;

                    switch (req.Url.AbsolutePath) {
                        case "/implicit":
                        case "/implicit/":
                            //Send js for forwarding code
                            string respPage = global::Clippy.Properties.Resources.RedirectPage;
                            byte[] buf = System.Text.Encoding.UTF8.GetBytes(respPage);
                            resp.ContentLength64 = buf.Length;
                            System.IO.Stream output = resp.OutputStream;
                            output.Write(buf, 0, buf.Length);
                            output.Close();

                            resp.Close();
                            break;
                        case "/submitcode":
                        case "/submitcode/":
                            //Handle forwarded code from our js
                            NameValueCollection query = HttpUtility.ParseQueryString(req.Url.Query);
                            string code = query.Get("access_token");
                            if (code != null && code != "") {
                                resp.StatusCode = 200;
                                resp.Close();
                                return code;
                            } else {
                                resp.StatusCode = 500;
                                resp.Close();
                                return null;
                            }
                    }
                }
            } finally {
                listener.Close();
            }
        }

        private void OpenBrowser(string url) {
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = url;
            System.Diagnostics.Process.Start(psi);
        }
    }
}
