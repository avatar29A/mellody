using System.Collections.Generic;
using Hqub.Mellody.Music.Services.Echonest;

namespace Hqub.Mellody.Music.Services.Interfaces
{
    public interface IEchonestService
    {
        EchoPlaylist GetPlaylistByGenre(IEnumerable<string> genres, int count);
    }
}
