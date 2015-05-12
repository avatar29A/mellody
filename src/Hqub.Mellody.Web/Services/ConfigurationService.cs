using Hqub.Mellody.Music.Configure;
using Hqub.Mellody.Music.Services;

namespace Hqub.Mellody.Web.Services
{
    public class ConfigurationService : IConfigurationService
    {
        public LastfmConfigureSection GetLastfmConfig()
        {
            var config = (LastfmConfigureSection)
                System.Configuration.ConfigurationManager.GetSection("mellodySectionGroup/lastfmSection");

            return config;
        }

        public YoutubeConfigureSection GetYoutubeConfig()
        {
            var config = (YoutubeConfigureSection)
               System.Configuration.ConfigurationManager.GetSection("mellodySectionGroup/youtubeSection");

            return config;
        }
    }
}