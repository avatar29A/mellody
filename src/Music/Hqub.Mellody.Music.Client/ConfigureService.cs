using Hqub.Mellody.Music.Configure;

namespace Hqub.Mellody.Music.Client
{
    public class ConfigureService : Services.IConfigurationService
    {
        public LastfmConfigureSection GetLastfmConfig()
        {
            var config = (LastfmConfigureSection)
               System.Configuration.ConfigurationManager.GetSection("customSectionGroup/lastfmSection");

            return config;
        }

        public YoutubeConfigureSection GetYoutubeConfig()
        {
            var config = (YoutubeConfigureSection)
               System.Configuration.ConfigurationManager.GetSection("customSectionGroup/youtubeSection");

            return config;
        }

        public PlaylistConfigureSection GetPlaylistConfig()
        {
            var config = (PlaylistConfigureSection)
              System.Configuration.ConfigurationManager.GetSection("customSectionGroup/playlistSection");

            return config;
        }
    }
}
