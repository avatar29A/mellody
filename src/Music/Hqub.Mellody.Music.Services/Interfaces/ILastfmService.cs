using System.Collections.Generic;
using DotLastFm.Models;

namespace Hqub.Mellody.Music.Services
{
    public interface ILastfmService
    {
        /// <summary>
        /// Use the last.fm corrections data to check whether the supplied artist has a correction to a canonical artist
        /// </summary>
        /// <param name="artistName">Artist name</param>
        /// <returns>Correct artist name</returns>
        string GetCorrection(string artistName);

        /// <summary>
        /// Get artist biography.
        /// </summary>
        /// <param name="artistName">Artist name</param>
        /// <param name="lang">Language text</param>
        /// <returns>return artist biography</returns>
        string GetBio(string artistName, string lang);

        /// <summary>
        /// Get artist full info.
        /// </summary>
        /// <param name="artistName"></param>
        /// <param name="lang"></param>
        /// <returns>full details by artist.</returns>
        ArtistWithDetails GetInfo(string artistName, string lang = "en");

        /// <summary>
        /// Get similar artists.
        /// </summary>
        /// <param name="artistName">aritst name</param>
        /// <param name="lang">language</param>
        /// <returns>list of similar artists</returns>
        List<ArtistSimilarArtist> GetSimilar(string artistName, string lang = "en");
    }
}
