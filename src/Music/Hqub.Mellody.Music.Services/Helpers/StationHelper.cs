using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hqub.Mellody.Music.Store.Models;

namespace Hqub.Mellody.Music.Helpers
{
    public static class StationHelper
    {
        /// <summary>
        /// Return guid of the string representation.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static Guid StringToGuid(string val)
        {
            Guid guidId;

            if (!string.IsNullOrEmpty(val) && Guid.TryParse(val, out guidId))
            {
                return guidId;
            }

            return Guid.Empty;
        }

        /// <summary>
        /// Generate station name from names plalists.
        /// </summary>
        /// <param name="playlists"></param>
        /// <returns></returns>
        public static string GenerateStationName(List<Playlist> playlists)
        {
            return string.Join(";", playlists.Select(p => p.Name));
        }
    }
}
