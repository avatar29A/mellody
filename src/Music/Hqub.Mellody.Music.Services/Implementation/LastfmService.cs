using System;
using System.Collections.Generic;
using System.Linq;
using Hqub.Mellody.Music.Configure;
using Lastfm.Services;

namespace Hqub.Mellody.Music.Services
{
    public class LastfmService : ILastfmService
    {
        private LastfmConfigureSection _configure;
        private Session _session;

        public LastfmService(IConfigurationService configurationService)
        {
            _configure = configurationService.GetLastfmConfig();

            _session = new Session(_configure.ApiKey, _configure.ApiSecret);
        }


        public Artist GetInfo(string artistName)
        {
            var artist = new Artist(artistName, _session);

            return artist;
        }

        public Artist GetInfoByMbId(string mbid)
        {
            var artist = Artist.GetByMBID(mbid, _session);

            return artist;
        }

        public List<Track> GetArtistTracks(string mbid)
        {
            var artist = GetInfoByMbId(mbid);

            var trackSearch = Track.Search(artist.Name, _session);

            var perPage = trackSearch.GetItemsPerPage();
            var resultCount = trackSearch.GetResultCount();

            var trackList = new List<Track>();
            for (int i = 1; i <= resultCount/perPage; i++)
            {
                trackList.AddRange(trackSearch.GetPage(i));
            }

            return trackList;
        }

        public List<Track> GetAlbumTracks(string albumId)
        {
            var album = Album.GetByMBID(albumId, _session);

            return album.GetTracks().ToList();
        }
    }
}
