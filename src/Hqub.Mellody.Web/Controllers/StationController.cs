using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Hqub.Mellody.Music.Services;
using Hqub.Mellody.Music.Store.Models;
using Hqub.Mellody.Poco;
using Hqub.Mellody.Web.Extensions;
using Hqub.Mellody.Web.Models.Response;

namespace Hqub.Mellody.Web.Controllers
{
    public class StationController : Controller
    {
        private readonly IStationService _stationService;
        private readonly IYoutubeService _youtubeService;
        private readonly ILastfmService _lastfmService;
        private readonly ICacheService _cacheService;

        public StationController(IStationService stationService,
            IYoutubeService youtubeService,
            ILastfmService lastfmService,
            ICacheService cacheService)
        {
            _stationService = stationService;
            _youtubeService = youtubeService;
            _lastfmService = lastfmService;
            _cacheService = cacheService;
        }

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

        private List<TrackDTO> FillExtInfoSection(List<TrackDTO> tracks)
        {
            foreach (var track in tracks)
            {
               var results = _youtubeService.Search(track.ToString());
                if (results.Count == 0)
                    continue;

                track.VideoId = results[0].VideoId;
                track.ArtistBio = _lastfmService.GetBio(track.Artist, "ru");
            }

            return tracks;
        }

        private List<TrackDTO> GetShuffleTracks(Guid stationId)
        {
            var tracks = _stationService.GetTracks(stationId);
            var tracksDTO = tracks.Select(Mapper.Map<TrackDTO>).ToList();

//            tracksDTO.Shuffle();

            return tracksDTO;
        }

        #region Methods

       

        #endregion
    }
}