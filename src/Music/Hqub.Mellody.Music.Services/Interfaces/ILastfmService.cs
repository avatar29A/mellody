using System.Collections.Generic;
using DotLastFm.Models;
using Artist = Lastfm.Services.Artist;
using Track = Lastfm.Services.Track;

namespace Hqub.Mellody.Music.Services
{
    public interface ILastfmService
    {
        /// <summary>
        /// Get artist info by MusicBrainz ID
        /// </summary>
        /// <param name="mbId"></param>
        /// <returns></returns>
        Artist GetInfo(string artistName);

        ArtistWithDetails GetInfoFull(string artistName, string lang = "en");


        Artist GetInfoByMbId(string mbid);

        /// <summary>
        /// Get tracks assigned with artist id
        /// </summary>
        /// <param name="artistId">MBID artist</param>
        /// <returns>list of tracks</returns>
        List<Track> GetArtistTracks(string artistId);

        /// <summary>
        /// Get tracks assigned with album id
        /// </summary>
        /// <param name="albumId">MBID album</param>
        /// <returns>list of tracks</returns>
        List<Track> GetAlbumTracks(string albumId);
    }
}
