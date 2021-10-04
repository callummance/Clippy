using Clippy.UrlParamEncoder;
using System;

namespace Clippy.TwitchApi{
    class GetStreamsOpts {
        [UrlParam("after")]
        public string After { get; set; }

        [UrlParam("before")]
        public string Before { get; set; }

        [UrlParam("first")]
        public Nullable<int> First { get; set; }

        [UrlParam("game_id")]
        public string GameId { get; set; }

        [UrlParam("language")]
        public string Language { get; set; }

        [UrlParam("user_id")]
        public string UserId { get; set; }

        [UrlParam("user_login")]
        public string UserLogin { get; set; }
    }
}
