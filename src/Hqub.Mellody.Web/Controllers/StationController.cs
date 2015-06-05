using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using DotLastFm.Models;
using Hqub.Mellody.Music.Services;
using Hqub.Mellody.Music.Services.Exceptions;
using Hqub.Mellody.Music.Services.Interfaces;
using Hqub.Mellody.Poco;
using Hqub.Mellody.Web.Extensions;
using Hqub.Mellody.Web.Models.Response;
using NLog.Fluent;
using Artist = Lastfm.Services.Artist;
using ImageSize = Lastfm.Services.ImageSize;
using Playlist = Hqub.Mellody.Music.Store.Models.Playlist;

namespace Hqub.Mellody.Web.Controllers
{
    public class StationController : Controller
    {
        private const int MaxQueryCount = 3;

        private readonly IStationService _stationService;
        private readonly IPlaylistService _playlistService;
        private readonly IYoutubeService _youtubeService;
        private readonly ILastfmService _lastfmService;
        private readonly IEchonestService _echonestService;
        private readonly ILogService _logService;

        public StationController(IStationService stationService,
            IPlaylistService playlistService,
            IYoutubeService youtubeService,
            ILastfmService lastfmService,
            IEchonestService echonestService,
            ILogService logService)
        {
            _stationService = stationService;
            _playlistService = playlistService;
            _youtubeService = youtubeService;
            _lastfmService = lastfmService;
            _echonestService = echonestService;
            _logService = logService;
        }


        #region Controller Methods

        [HttpGet]
        public ActionResult Index(string id)
        {
            var guid = Music.Helpers.StationHelper.StringToGuid(id);
            if (guid == Guid.Empty)
                return RedirectToAction("Index", "Playlist");

            return View(new StationDTO
            {
                Id = guid
            });
        }

        /// <summary>
        /// Return all tracks by station id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get(Guid id)
        {
            try
            {
                const int countTrackPerRequest = 1;
                var stationName = string.Format("station_{0}", id);

                List<TrackDTO> tracksDTO;

                //Try get tracks from session
                if (Session[stationName] == null)
                    tracksDTO = GetShuffleTracks(id);
                else // if track list from session is empty, then get tracks from DB.
                {
                    tracksDTO = (List<TrackDTO>) Session[stationName];
                    if (tracksDTO == null || tracksDTO.Count == 0)
                        tracksDTO = GetShuffleTracks(id);
                }

                // Remove first 'countTrackPerRequest' tracks from playlist.
                Session[stationName] = tracksDTO.Skip(countTrackPerRequest).ToList();

                var portionTracks = FillExtInfoSection(tracksDTO.Take(countTrackPerRequest).ToList());
                // Update last listen tracks:
                var historyTracks = SetLastListenTracks(portionTracks);
                var historyStations = SetLastStations(new StationDTO
                {
                    Name = _stationService.GetName(id),
                    Id = id
                });

                return Json(new PlaylistResponse
                {
                    StationName = _stationService.GetName(id),
                    Tracks = portionTracks,
                    HistoryTracks = historyTracks,
                    HistoryStations = historyStations
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.AddExceptionFull(string.Format("Controller: StationController, Action: Get. Args=[id:{0}]", id),
                    ex);

                return Json(new ResponseEntity
                {
                    IsError = true,
                    Message = "Internal server error.",
                    StatusCode = 500
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Return history stations
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetHistoryStations()
        {
            try
            {
                var stations = GetFromSession<List<StationDTO>>(Keys.HistoryStations) ?? new List<StationDTO>();

                return Json(new GetHistoryStationsResponse(stations), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Logger.AddExceptionFull("StationController.GetStationHistory", exception);

                return Json(new GetHistoryStationsResponse(new List<StationDTO>())
                {
                    IsError = true
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Parse query, create station and save in db.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Create(Models.PrepareRadioModel model)
        {
            try
            {
                var queries = model.Queries.Take(MaxQueryCount).ToList();

                var playlists = new List<Playlist>();
                foreach (var query in queries)
                {
                    var playlist = await _playlistService.Create(query);
                    if (playlist == null) // if tracks number is 0, then playlist is null.
                        continue;

                    playlists.Add(playlist);
                }

                if (playlists.Count == 0)
                    throw new EmptySearchResultException();

                // Create personal station:
                var stationId = _stationService.Create(playlists);

                return Json(new RadioCreatedResponse(stationId));
            }
            catch (EmptySearchResultException ex)
            {
                Logger.AddException(
                    string.Format("Not found queries:\n{0}",
                        string.Join("\n", model.Queries.Select(q => q.Name).ToArray())), ex);

                return Json(new RadioCreatedResponse
                {
                    IsError = true,
                    Message = "Not found queries",
                    StatusCode = 404
                });
            }
            catch (Exception exception)
            {
                Logger.AddExceptionFull("PlaylistController.Index [POST]", exception);

                return Json(new RadioCreatedResponse
                {
                    IsError = true,
                    Message = "Internal server error.",
                    StatusCode = 500
                });
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Save the last five tracks
        /// </summary>
        private List<TrackDTO> SetLastListenTracks(IEnumerable<TrackDTO> tracks)
        {
            var lastListenTracks = GetFromSession<List<TrackDTO>>(Keys.HistorySongs) ?? new List<TrackDTO>();

            lastListenTracks.InsertRange(0, tracks.Where(t => !string.IsNullOrEmpty(t.VideoId)));

            Session[Keys.HistorySongs] = lastListenTracks.Take(5).ToList();

            return lastListenTracks;
        }


        /// <summary>
        /// Save the last five stations
        /// </summary>
        private List<StationDTO> SetLastStations(StationDTO station)
        {
            var lastStationList = GetFromSession<List<StationDTO>>(Keys.HistoryStations) ?? new List<StationDTO>();
            if (lastStationList.Any(s => s.Id == station.Id))
                return lastStationList;

            lastStationList.Insert(0, station);

            Session[Keys.HistoryStations] = lastStationList.Take(5).ToList();

            return lastStationList;
        }


        /// <summary>
        /// Common method for extract data from session.
        /// </summary>
        private T GetFromSession<T>(string sessionKey)
        {
            return (T) Session[sessionKey];
        }

        /// <summary>
        /// Add to response:
        /// - Biography by artist
        /// - Similar tracks,
        /// - Artist and track name
        /// - Youtube video ID
        /// </summary>
        /// <param name="tracks"></param>
        /// <returns></returns>
        private List<TrackDTO> FillExtInfoSection(List<TrackDTO> tracks)
        {
            foreach (var track in tracks)
            {
                var results = _youtubeService.Search(track.ToString());
                if (results.Count == 0)
                    continue;

                track.VideoId = GetMaximalSimiliarVideo(results);

                try
                {
                    var artistInfo = _lastfmService.GetInfoFull(track.Artist, "ru");

                    track.ArtistBio = artistInfo.Bio.Summary;
                    track.ImageUrl = GetArtistImage(artistInfo.Images);
                    track.SimilarArtists = GetSimilarArtists(artistInfo.SimilarArtists);

                    // Get similar tags from Echonest service:
                    track.Tags = _echonestService.GetSimilarGenres(track.Artist);
                }
                catch (Exception exception)
                {
                    _logService.AddExceptionFull(string.Format("FillExtInfoSection. Track ({0})", track.FullTitle), exception);
                }
            }

            return tracks;
        }

        private string GetMaximalSimiliarVideo(IEnumerable<YoutubeVideoDTO> videos)
        {
            var video = videos.OrderByDescending(v => v.Rank).First();
            if (video.Rank != 100)
                return string.Empty;

            return video.VideoId;
        }


        /// <summary>
        /// Return url image
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        private string GetArtistImage(IEnumerable<Image> images)
        {
            return images.First(img => img.Size == DotLastFm.Models.ImageSize.ExtraLarge).Value;
        } 


        /// <summary>
        /// List of similar artists
        /// </summary>
        /// <param name="similarArtists"></param>
        /// <returns></returns>
        private List<ArtistDTO> GetSimilarArtists(IEnumerable<ArtistSimilarArtist> similarArtists)
        {
            return new List<ArtistDTO>(similarArtists.Select(a => new ArtistDTO
            {
                ArtistName = a.Name,
                ImageUrl = GetArtistImage(a.Images)
            }));
        }

        /// <summary>
        /// Randomize playlist
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        private List<TrackDTO> GetShuffleTracks(Guid stationId)
        {
            var tracks = _stationService.GetTracks(stationId);
            var tracksDTO = tracks.Select(Mapper.Map<TrackDTO>).ToList();

            tracksDTO.Shuffle();

            return tracksDTO;
        }

        #endregion
    }
}