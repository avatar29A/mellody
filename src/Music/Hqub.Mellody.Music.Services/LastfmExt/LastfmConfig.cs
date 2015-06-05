using DotLastFm.Api;

namespace Hqub.Mellody.Music.Services.LastfmExt
{
    public class LastFmConfig : ILastFmConfig
    {
        public LastFmConfig(string apiKey, string secret)
        {
            BaseUrl = "http://ws.audioscrobbler.com/2.0";
            ApiKey = apiKey;
            Secret = secret;
        }

        public string BaseUrl { get; private set; }
        public string ApiKey { get; private set; }
        public string Secret { get; private set; }
    }
}
