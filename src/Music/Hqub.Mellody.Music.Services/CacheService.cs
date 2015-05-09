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
        public bool Exists(List<Poco.QueryEntity> query)
        {
            var hash = GetHash(GetQueryAsString(query));

            using (var ctx = MusicStoreDbContext.GetContext())
            {
                return ctx.Playlists.Any(x => x.Hash == hash);
            }
        }

        public Playlist GetPlaylist(List<QueryEntity> query)
        {
            var stringfityQuery = GetQueryAsString(query);
            var hash = GetHash(stringfityQuery);

            using (var ctx = MusicStoreDbContext.GetContext())
            {
                var playlist = ctx.Playlists.FirstOrDefault(q => q.Hash == hash);
                return playlist;
            }
        }

        public void AddPlaylist(List<QueryEntity> query, Guid playlistId)
        {
            var stringfityQuery = GetQueryAsString(query);
            var hash = GetHash(stringfityQuery);

            using (var ctx = new MusicStoreDbContext())
            {
                var playlist = ctx.Playlists.First(x => x.Id == playlistId);
                playlist.Name = stringfityQuery;
                playlist.Hash = hash;

                ctx.SaveChanges();
            }
        }

        private string GetQueryAsString(IEnumerable<QueryEntity> query)
        {
            var joinQuery = string.Join(";", query.OrderBy(x => x.Name).Select(x => string.Format("{0}_{1}",
                 Enum.GetName(typeof(TypeQuery), x.TypeQuery), ClearName(x.Name, x.TypeQuery))));

            return joinQuery;
        }

        private string ClearName(string name, TypeQuery type)
        {
            if (type == TypeQuery.Artist || type == TypeQuery.Query)
                return Normalize(name);
            
            //if Album:
            var split = name.Split('-');

            return string.Join("-", split.Select(Normalize).ToArray());
        }

        private string Normalize(string name)
        {
            return name.Trim().ToLower();
        }

        private string GetHash(string query)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(query);
            var hashstring = new SHA256Managed();
            var hash = hashstring.ComputeHash(bytes);
            var hashString = hash.Aggregate(string.Empty, (current, x) => current + String.Format("{0:x2}", x));

            return hashString;
        }
    }
}
