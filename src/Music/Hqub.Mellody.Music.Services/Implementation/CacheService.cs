using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Hqub.Mellody.Music.Store;
using Hqub.Mellody.Music.Store.Models;
using Hqub.Mellody.Poco;

namespace Hqub.Mellody.Music.Services
{
    public class CacheService : ICacheService
    {
        #region ICacheService

        public bool ExistsPlaylist(QueryEntity query)
        {
            var hash = Helpers.HashAlgHelper.GetHash(Helpers.PlaylistHelper.ConvertQueryToString(query));

            using (var ctx = MusicStoreDbContext.GetContext())
            {
                return ctx.Playlists.Any(x => x.Hash == hash);
            }
        }

        public Playlist GetPlaylist(QueryEntity query)
        {
            var hash = Helpers.HashAlgHelper.GetHash(Helpers.PlaylistHelper.ConvertQueryToString(query));

            using (var ctx = MusicStoreDbContext.GetContext())
            {
                var playlist = ctx.Playlists.FirstOrDefault(q => q.Hash == hash);
                return playlist;
            }
        }

        public void AddPlaylist(QueryEntity query, Guid id)
        {
            var playlistName = Helpers.PlaylistHelper.ConvertQueryToString(query);
            var hash = Helpers.HashAlgHelper.GetHash(playlistName);

            using (var ctx = new MusicStoreDbContext())
            {
                var playlist = ctx.Playlists.First(x => x.Id == id);
                playlist.Name = playlistName;
                playlist.Hash = hash;

                ctx.SaveChanges();
            }
        }

        public Playlist AddPlaylist(QueryEntity query, Playlist playlist)
        {
            var playlistName = Helpers.PlaylistHelper.ConvertQueryToString(query);
            var hash = Helpers.HashAlgHelper.GetHash(playlistName);

            playlist.HashDescription = playlistName;
            playlist.Hash = hash;

            return playlist;
        }

        #endregion
    }
}
