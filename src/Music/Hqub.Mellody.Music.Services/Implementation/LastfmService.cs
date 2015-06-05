using System;
using System.Collections.Generic;
using System.Linq;
using DotLastFm.Models;
using Hqub.Mellody.Music.Configure;
using Hqub.Mellody.Music.Services.LastfmExt;
using Lastfm.Services;
using Album = Lastfm.Services.Album;
using Artist = Lastfm.Services.Artist;
using Track = Lastfm.Services.Track;

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

        public ArtistWithDetails GetInfoFull(string artistName, string lang = "en")
        {
            var client = new DotLastFm.LastFmApi(new LastFmConfig(_configure.ApiKey, _configure.ApiSecret));

            var artistApi = new ExtendArtistApi(client);
            var info = artistApi.GetInfo(artistName, true, lang, null);

            return info;
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
            trackSearch.SpecifyItemsPerPage(100);

            var trackList = new List<Track>();
            for (int i = 1; i <= 3; i++)
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
