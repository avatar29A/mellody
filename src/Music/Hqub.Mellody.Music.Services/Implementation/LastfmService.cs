using DotLastFm.Api;
using DotLastFm.Api.Rest;
using DotLastFm.Models;
using DotLastFm.Models.Wrappers;
using Hqub.Mellody.Music.Configure;

namespace Hqub.Mellody.Music.Services
{
    public class LastfmService : ILastfmService
    {
        #region Nested class

        class LastFmConfig : ILastFmConfig
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

        public class ExtendArtistApi : ArtistApi 
        {
            public ExtendArtistApi(ILastFmApi api) : base(api)
            {
            }

            public ArtistWithDetails GetInfo(string artist, bool autocorrect, string lang, string username)
            {
                var call = Rest.Method("artist.getInfo")
                               .AddParam("artist", artist)
                               .AddParam("lang", lang)
                               .AddParam("autocorrect", autocorrect ? "1" : "0");

                if (username != null)
                {
                    call.AddParam("username", username);
                }

                var wrapper = call.Execute<ArtistWithDetailsWrapper>();

                if (wrapper != null)
                {
                    return wrapper.Artist;
                }

                return null;
            } 
        }

        #endregion

        private LastfmConfigureSection _configure;
        public LastfmService(IConfigurationService configurationService)
        {
            _configure = configurationService.GetLastfmConfig();
        }

        public string GetCorrection(string artistName)
        {
            throw new System.NotImplementedException();
        }

        public string GetBio(string artistName, string lang="en")
        {
            var client = new DotLastFm.LastFmApi(new LastFmConfig(_configure.ApiKey, _configure.ApiSecret));

            var artistApi = new ExtendArtistApi(client);
            var info  = artistApi.GetInfo(artistName, true, lang, null);

            return info.Bio.Summary;
        }
    }
}
