using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Hqub.Mellody.Music.Services;
using Hqub.Mellody.Music.Services.Exceptions;
using Hqub.Mellody.Web.Models.Radio;
using Hqub.Mellody.Web.Models.Response;
using Microsoft.Practices.Unity.Utility;

namespace Hqub.Mellody.Web.Controllers
{
    public class RadioController : Controller
    {
        private const int MaxQueryCount = 5;

        private readonly IPlaylistService _playlistService;
        private readonly IStationService _stationService;
        private readonly ICacheService _cacheService;

        public RadioController(IPlaylistService playlistService,
            IStationService stationService,
            ICacheService cacheService)
        {
            _playlistService = playlistService;
            _stationService = stationService;
            _cacheService = cacheService;
        }

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
                var playlist = _playlistService.GetPlaylist(id);

                return Json(playlist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(null);
            }
        }

        // GET: Radio
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Station(string id)
        {
            var guid = StringToGuid(id);

            if (guid == Guid.Empty)
                return RedirectToAction("Index");

            return View(new StationModel
            {
                StationId = id
            });
        }

        private Guid StringToGuid(string val)
        {
            Guid guidId;

            if (!string.IsNullOrEmpty(val) && Guid.TryParse(val, out guidId))
            {
                return guidId;
            }

            return Guid.Empty;
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

                var playlist = _cacheService.GetPlaylist(queries);
                if (playlist != null)
                {
                    return Json(new RadioCreatedResponse(playlist.Id));
                }

                // Get tracks for playlist:
                var tracks = await _playlistService.CreatePlaylist(queries);

                // Save playlist and get id:
                var stationId = _stationService.Create(tracks);

                // Save playlist in cache:
                _cacheService.AddPlaylist(queries, stationId);

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
                Logger.AddException(exception);

                return Json(new RadioCreatedResponse
                {
                    IsError = true,
                    Message = "Internal server error.",
                    StatusCode = 500
                });
            }
        }

        [HttpPost]
        public JsonResult Check(Models.PrepareRadioModel model)
        {
            return Json(null);
        }
    }
}