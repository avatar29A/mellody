using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hqub.Mellody.Music.Store.Models;

namespace Hqub.Mellody.Music.Store
{
    public class MusicStoreDbContext : DbContext
    {
        public MusicStoreDbContext()
            : base("MusicStoreEntities")
        {
        }

        public static MusicStoreDbContext GetContext()
        {
            return new MusicStoreDbContext();
        }

        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Track> Tracks { get; set; }
    }
}
