using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using Clippy.TwitchApi;
using NLog;

namespace Clippy {
    class TwitchClipper {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private TwitchApiClient client;

        private string lastTwitchUserName;
        private string lastTwitchChannelId;


        public TwitchClipper(ITwitchApiAuth client) {
            this.client = new TwitchApiClient(client);
        }

        public async Task<ClipCreateResponse> NewClipAsync(string channelUserName) {
            try {
                if (channelUserName != lastTwitchUserName || lastTwitchChannelId == null || lastTwitchChannelId == "") {
                    //We don't already have the channel id, so try and find it
                    List<StreamData> streams = await this.client.GetStreams(new GetStreamsOpts() {
                        UserLogin = channelUserName,
                    });
                    if (streams.Count == 0) {
                        logger.Warn($"Found zero matches for channel owned by user {channelUserName}. This may be because they are not currently streaming.");
                        return null;
                    } else if (streams.Count > 1) {
                        logger.Warn($"Found multiple matches for channel owned by user {channelUserName}. Using the first one returned...");
                        this.lastTwitchUserName = channelUserName;
                        this.lastTwitchChannelId = streams[0].UserId;
                    } else {
                        this.lastTwitchUserName = channelUserName;
                        this.lastTwitchChannelId = streams[0].UserId;
                    }
                }

                //Create the clip
                ClipCreateResponse res = await this.client.CreateClip(this.lastTwitchChannelId, true);
                return res;
            } catch (HttpRequestException e) {
                logger.Error(e, "Failed to create clip.");
                return null;
            }
        }

    }
}
