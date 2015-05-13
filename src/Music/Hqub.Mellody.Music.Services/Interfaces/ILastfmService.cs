using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
