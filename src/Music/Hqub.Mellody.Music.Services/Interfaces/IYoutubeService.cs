using System.Collections.Generic;
using Hqub.Mellody.Poco;

namespace Hqub.Mellody.Music.Services
{
    public interface IYoutubeService
    {
        List<SearchTrackDTO> Search(string query);
    }
}
