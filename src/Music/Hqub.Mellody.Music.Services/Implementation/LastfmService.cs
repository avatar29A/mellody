using Hqub.Mellody.Music.Configure;
using Hqub.Mellody.Music.Services.Lastfm;

namespace Hqub.Mellody.Music.Services
{
    public class LastfmService : ILastfmService
    {
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
