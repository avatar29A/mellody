using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using EntityFramework.BulkInsert.Extensions;
using Hqub.Mellody.Music.Store;
using Hqub.Mellody.Music.Store.Models;

namespace Hqub.Mellody.Music.Services
{
    public class StationService : IStationService
    {
        public Guid Create(List<Poco.Track> tracks)
        {
            var playlistId = Guid.NewGuid();

#if DEBUG
            var stopwatch = new Stopwatch();
            stopwatch.Start();
#endif

            using (var ctx = new MusicStoreDbContext())
            {
                using (var transactionScope = new TransactionScope())
                {
                    var playlist = new Playlist
                    {
                        Id = playlistId,
                    };

                    ctx.Playlists.Add(playlist);

                    playlist.Tracks = new List<Track>(tracks.Select(t => new Track
                    {
                        Id = Guid.NewGuid(),
                        Artist = t.Artist,
                        Title = t.Title,
                        Duration = t.Duration,
                        Playlist = playlist

                    }));

                    ctx.SaveChanges();
                    transactionScope.Complete();
                }
            }

#if DEBUG
            stopwatch.Stop();
            Debug.WriteLine("QUERY END FOR: {0}", stopwatch.Elapsed);
#endif

            return playlistId;
        }
    }
}