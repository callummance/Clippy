using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Clippy.TwitchApi {
    interface ITwitchApiAuth {
        Task<HttpResponseMessage> MakeReq(string url, HttpMethod method);
    }
}
