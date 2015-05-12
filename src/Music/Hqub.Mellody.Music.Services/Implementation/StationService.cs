using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Hqub.Mellody.Music.Store;
using Hqub.Mellody.Music.Store.Models;

namespace Hqub.Mellody.Music.Services
{
    public class StationService : IStationService
    {
        #region IStationService

        public Guid Create(List<Playlist> playlists)
        {
            var stationId = Guid.NewGuid();

            using (var ctx = new MusicStoreDbContext())
            {
                var station = new Station
                {
                    Id = stationId,
                    Name = Helpers.StationHelper.GenerateStationName(playlists),
                    Playlists = new Collection<Playlist>(playlists)
                };


                foreach (var playlist in playlists)
                {
                    ctx.Playlists.Attach(playlist);
                }

                ctx.Stations.Add(station);
                ctx.SaveChanges();
            }

            return stationId;
        }

        public Station Get(Guid id)
        {
            using (var ctx = MusicStoreDbContext.GetContext())
            {
                return ctx.Stations.First(s => s.Id == id);
            }
        }

        public IList<Track> GetTracks(Guid stationId)
        {
            using (var ctx = MusicStoreDbContext.GetContext())
            {
                var station =  ctx.Stations.First(s => s.Id == stationId);

                return station.Playlists.SelectMany(p => p.Tracks).ToList();
            }
        }

        #endregion
    }
}