using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Hqub.Mellody.Music.Services;
using Hqub.Mellody.Web.Models.Radio;
using Microsoft.Practices.Unity.Utility;

namespace Hqub.Mellody.Web.Controllers
{
    public class RadioController : Controller
    {
        private readonly IPlaylistService _playlistService;
        private readonly IStationService _stationService;

        public RadioController(IPlaylistService playlistService, IStationService stationService)
        {
            _playlistService = playlistService;
            _stationService = stationService;
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
                // Get tracks for playlist:
                var playlist = await _playlistService.CreatePlaylist(model.Queries);

                // Save playlist and get id:
                var stationId = _stationService.Create(playlist);

                return Json(new Models.Response.RadioCreatedResponse(stationId));
            }
            catch (Exception exception)
            {
                return Json(new Models.Response.RadioCreatedResponse
                {
                    IsError = true,
                    Message = ""
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