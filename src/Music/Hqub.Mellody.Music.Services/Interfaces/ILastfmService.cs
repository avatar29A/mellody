using System.Collections.Generic;
using Lastfm.Services;

namespace Hqub.Mellody.Music.Services
{
    public interface ILastfmService
    {
        /// <summary>
        /// Get artisti info by MusicBrainz ID
        /// </summary>
        /// <param name="mbId"></param>
        /// <returns></returns>
        Artist GetInfo(string artistName);

        Artist GetInfoByMbId(string mbid);


    }
}
