using Hqub.Mellody.Music.Configure;
using Hqub.Mellody.Music.Services;

namespace Hqub.Mellody.Web.Services
{
    public class ConfigurationService : IConfigurationService
    {
        public PythonConfigureSection GetVkontakteAuthConfig()
        {
            var config = (PythonConfigureSection)
               System.Configuration.ConfigurationManager.GetSection("mellodySectionGroup/authSection");

            return config;
        }

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

        public PlaylistConfigureSection GetPlaylistConfig()
        {
            var config =
                (PlaylistConfigureSection)
                    System.Configuration.ConfigurationManager.GetSection("mellodySectionGroup/playlistSection");

            return config;
        }

        public EchonestConfigureSection GetEchonestConfig()
        {
            var config =
                (EchonestConfigureSection)
                    System.Configuration.ConfigurationManager.GetSection("mellodySectionGroup/echonestSection");

            return config;
        }
    }
}