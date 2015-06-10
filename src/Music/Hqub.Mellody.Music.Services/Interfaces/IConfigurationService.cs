using Hqub.Mellody.Music.Configure;

namespace Hqub.Mellody.Music.Services
{
    public interface IConfigurationService
    {
        PythonConfigureSection GetVkontakteAuthConfig();
        LastfmConfigureSection GetLastfmConfig();
        YoutubeConfigureSection GetYoutubeConfig();
        PlaylistConfigureSection GetPlaylistConfig();
        EchonestConfigureSection GetEchonestConfig();
    }
}
