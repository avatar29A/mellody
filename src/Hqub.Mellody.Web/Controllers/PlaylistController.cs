using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Hqub.Mellody.Music.Services;
using Hqub.Mellody.Music.Services.Exceptions;
using Hqub.Mellody.Music.Store.Models;
using Hqub.Mellody.Poco;
using Hqub.Mellody.Web.Models.Radio;
using Hqub.Mellody.Web.Models.Response;
using Microsoft.Practices.Unity.Utility;

namespace Hqub.Mellody.Web.Controllers
{
    public class PlaylistController : Controller
    {
        private const int MaxQueryCount = 3;

        private readonly IPlaylistService _playlistService;
        private readonly IStationService _stationService;
        private readonly ICacheService _cacheService;

        public PlaylistController(IPlaylistService playlistService,
            IStationService stationService,
            ICacheService cacheService)
        {
            _playlistService = playlistService;
            _stationService = stationService;
            _cacheService = cacheService;
        }

        #region API

        /// <summary>
        /// Return all tracks by playlist id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetPlaylist(Guid id)
        {
            try
            {
                var station = _stationService.Get(id);
                var tracks = station.Playlists.Select(p => p.Tracks);

                return Json(tracks, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(null);
            }
        }

        #endregion

        // GET: Radio
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Parse query, create station and save in db.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Index(Models.PrepareRadioModel model)
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

                if(playlists.Count == 0)
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
    }
}