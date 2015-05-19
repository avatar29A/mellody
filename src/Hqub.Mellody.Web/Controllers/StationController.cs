using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using DotLastFm.Models;
using Hqub.Mellody.Music.Services;
using Hqub.Mellody.Music.Services.Exceptions;
using Hqub.Mellody.Music.Store.Models;
using Hqub.Mellody.Poco;
using Hqub.Mellody.Web.Extensions;
using Hqub.Mellody.Web.Models.Response;

namespace Hqub.Mellody.Web.Controllers
{
    public class StationController : Controller
    {
        private const int MaxQueryCount = 3;

        private readonly IStationService _stationService;
        private readonly IPlaylistService _playlistService;
        private readonly IYoutubeService _youtubeService;
        private readonly ILastfmService _lastfmService;
        private readonly ICacheService _cacheService;

        public StationController(IStationService stationService,
            IPlaylistService playlistService,
            IYoutubeService youtubeService,
            ILastfmService lastfmService,
            ICacheService cacheService)
        {
            _stationService = stationService;
            _playlistService = playlistService;
            _youtubeService = youtubeService;
            _lastfmService = lastfmService;
            _cacheService = cacheService;
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
                return Json(new PlaylistResponse
                {
                    StationName = _stationService.GetName(id),
                    Tracks = portionTracks
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

        private List<TrackDTO> FillExtInfoSection(List<TrackDTO> tracks)
        {
            foreach (var track in tracks)
            {
                var results = _youtubeService.Search(track.ToString());
                if (results.Count == 0)
                    continue;

                track.VideoId = results[0].VideoId;

                var artistInfo = _lastfmService.GetInfo(track.Artist, "ru");
                track.ArtistBio = artistInfo.Bio.Summary;
                track.SimilarArtists = new List<ArtistDTO>(artistInfo.SimilarArtists.Select(a => new ArtistDTO
                {
                    ArtistName = a.Name,
                    ImageUrl = a.Images.First(img => img.Size == ImageSize.ExtraLarge).Value
                }));
                track.Tags = new List<string>
                {
                    "rock", "metal", "heavy-metal"
                };
            }

            return tracks;
        }

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