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
        private readonly ICacheService _cacheService;

        public StationController(IStationService stationService,
            IYoutubeService youtubeService,
            ICacheService cacheService)
        {
            _stationService = stationService;
            _youtubeService = youtubeService;
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

                var portionTracks = FillYoutubeSection(tracksDTO.Take(countTrackPerRequest).ToList());
                return Json(new PlaylistResponse
                {
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

        private List<TrackDTO> FillYoutubeSection(List<TrackDTO> tracks)
        {
            foreach (var track in tracks)
            {
               var results = _youtubeService.Search(track.ToString());
                if (results.Count == 0)
                    continue;

                track.VideoId = results[0].VideoId;
                track.ArtistBio = " Believe is the twenty-third studio album by American singer-actress Cher, released on November 10, 1998 by Warner Bros. Records. The RIAA certified it Quadruple Platinum on December 23, 1999, recognizing four million shipments in the United States; Worldwide, the album has sold more than 20 million copies, making it the biggest-selling album of her career. In 1999 the album received three Grammy Awards nominations including &quot;Record of the Year&quot;, &quot;Best Pop Album&quot; and winning &quot;Best Dance Recording&quot; for the single &quot;Believe&quot;.";
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

        #region Methods

       

        #endregion
    }
}