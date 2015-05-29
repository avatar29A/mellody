using System.Data.Entity;
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

        public DbSet<Station> Stations { get; set; } 
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Track> Tracks { get; set; }
    }
}
