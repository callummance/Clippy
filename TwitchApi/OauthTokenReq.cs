using Clippy.UrlParamEncoder;
using System;

namespace Clippy.TwitchApi {
    class OauthTokenReq {
        [UrlParam("client_id")]
        public string ClientId { get; set; }

        [UrlParam("client_secret")]
        public string ClientSecret { get; set; }

        [UrlParam("redirect_uri")]
        public string RedirectUri { get; set; }

        [UrlParam("response_type")]
        public string ResponseType{ get; set; }


        [UrlParam("grant_type")]
        public string GrantType { get; set; }

        [UrlParam("scope")]
        public string Scopes { get; set; }

        [UrlParam("force_verify")]
        public Nullable<bool> ForceVerify{ get; set; }

        [UrlParam("state")]
        public string State{ get; set; }
    }
}
