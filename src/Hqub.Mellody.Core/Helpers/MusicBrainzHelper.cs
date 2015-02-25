using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hqub.MusicBrainz.API.Entities;

namespace Hqub.Mellody.Core.Helpers
{
    public class MusicBrainzHelper
    {
        public static async Task<AlbumTracksAndInfo> GetAlbumTracks(string artistName, string albumName)
        {

            var artist = (await Artist.SearchAsync(artistName)).First();

            var query = string.Format("aid=({0}) release=({1})", artist.Id, albumName);
            var album = (await Release.SearchAsync(Uri.EscapeUriString(query), 10)).First();

            var release = await Release.GetAsync(album.Id, "recordings");

            var recordings = new List<string>();
            foreach (var medium in release.MediumList)
            {
                recordings.AddRange(medium.Tracks.Select(track => track.Recordring).Select(recording => recording.Title));
            }

            var albumDTO = new AlbumTracksAndInfo
            {
                Tracks = recordings,
                Artist = artist.Name,
                Album = release.Title,
                Date = release.Date
            };

            return albumDTO;
        }
    }

    public class AlbumTracksAndInfo
    {
        /// <summary>
        /// Треки
        /// </summary>
        public List<string> Tracks { get; set; }

        /// <summary>
        /// Дата издания
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Название альбома
        /// </summary>
        public string Album { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        public string Artist { get; set; }

        public string Year
        {
            get
            {
                DateTime d;
                return DateTime.TryParse(Date, out d) ? d.Year.ToString() : Date;
            }
        }

        public override string ToString()
        {


            return string.Format("Группа: {0}\nАльбом: {1}\nКол-во треков: {2}\nГод выпуска: {3}\n", Artist, Album, Tracks.Count, Year);
        }
    }
}
