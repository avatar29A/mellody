using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Hqub.Mellody.Music.Services;
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

        // GET: Radio
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetPlaylist(Guid playlistId)
        {
            return Json(null);
        }

        [HttpPost]
        public async Task<JsonResult> Index(Models.PrepareRadioModel model)
        {
            // Get tracks for playlist:
            var playlist = await _playlistService.CreatePlaylist(model.Queries);

            // Save playlist and get id:
            var stationId = _stationService.Create(playlist);

            return Json(new Models.Response.RadioCreatedResponse(stationId));
        }

        [HttpPost]
        public JsonResult Check(Models.PrepareRadioModel model)
        {
            return Json(null);
        }

        public ActionResult Station()
        {
            return View();
        }
    }
}