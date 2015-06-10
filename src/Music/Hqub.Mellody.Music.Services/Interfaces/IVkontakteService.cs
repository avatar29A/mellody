using System.Collections.Generic;
using Hqub.Mellody.Poco;

namespace Hqub.Mellody.Music.Services.Interfaces
{
    public interface IVkontakteService
    {
        List<SearchTrackDTO> SearchTracks(string query);
    }
}
