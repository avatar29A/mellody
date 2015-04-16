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

            var recordings = new List<Recording>();
            foreach (var medium in release.MediumList)
            {
                recordings.AddRange(medium.Tracks.Select(track => track.Recordring));
            }

            var albumDTO = new AlbumTracksAndInfo
            {
                Tracks = recordings,
                Artist = artist.Name,
                Album = release.Title,
                Date = release.Date,
            };

            return albumDTO;
        }

        public static async Task<ArtistInfo> GetArtistInfo(string artistName)
        {
            var artist = (await Artist.SearchAsync(artistName)).First();

            var query = string.Format("aid=({0})", artist.Id);
            var releases = (await Release.SearchAsync(Uri.EscapeUriString(query)));

            return new ArtistInfo
            {
                ArtistName = artist.Name,
                BeginYear = artist.LifeSpan.Begin,
                Tags = string.Join(", ", artist.Tags.Select(t => t.Name)),
                Albums = new List<Release>(releases.Select(r => r))
            };
        }

    }

    public class ArtistInfo
    {

        public string BeginYear { get; set; }
        public string ArtistName { get; set; }

        public List<Release> Albums { get; set; }

        public string Tags { get; set; }

        public string Bio { get; set; }

        public override string ToString()
        {
            var albumsListString = new StringBuilder();
            for (int i = 0; i < Albums.Count; i++)
            {
                var album = Albums[i];
                albumsListString.AppendFormat("{0}. {1} ({2})\n", i + 1, album.Title, album.Date);
            }

            return string.Format("Группа: {0}\nГод образования: {1}\nТэги: {2}\n[Биография]\n{3}\n[Альбомы]\n{4}",
                ArtistName, BeginYear, Tags, Bio, albumsListString);
        }
    }

    public class AlbumTracksAndInfo
    {
        /// <summary>
        /// Треки
        /// </summary>
        public List<Recording> Tracks { get; set; }

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
