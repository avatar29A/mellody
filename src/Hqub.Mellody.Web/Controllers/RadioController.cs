using System;
using System.Web.Mvc;
using Hqub.Mellody.Web.Services;
using Microsoft.Practices.Unity.Utility;

namespace Hqub.Mellody.Web.Controllers
{
    public class RadioController : Controller
    {
        private IPlaylistService _playlistService;

        public RadioController(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
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
        public JsonResult Index(Models.PrepareRadioModel model)
        {
            var playlist = _playlistService.CreatePlaylist(model.Queries);


            return Json(new Models.Response.RadioCreatedResponse());
        }

        [HttpPost]
        public JsonResult Check(Models.PrepareRadioModel model)
        {
            return Json(new Models.Response.RadioCreatedResponse());
        }

        public ActionResult Station()
        {
            return View();
        }
    }
}