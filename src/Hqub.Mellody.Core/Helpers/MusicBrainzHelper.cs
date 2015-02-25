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
        public static async Task<List<string>> GetAlbumTracks(string artistName, string albumName)
        {
            var artist = (await Artist.SearchAsync(artistName)).First();

            var query = string.Format("aid=({0}) release=({1})", artist.Id, albumName);
            var album = (await Release.SearchAsync(Uri.EscapeUriString(query), 10)).First();

            var release = await Release.GetAsync(album.Id, "recordings");

            var recordings = new List<string>();
            foreach (var medium in release.MediumList)
            {
                foreach (var track in medium.Tracks)
                {
                    var recording = track.Recordring;
                    recordings.Add(recording.Title);
                }
            }

            return recordings;
        }
    }
}
