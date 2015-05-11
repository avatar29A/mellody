using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Hqub.Mellody.Music.Services;
using Hqub.Mellody.Music.Store.Models;
using Hqub.Mellody.Poco;
using Hqub.Mellody.Web.Models.Response;

namespace Hqub.Mellody.Web.Controllers
{
    public class StationController : Controller
    {
        private readonly IStationService _stationService;
        private readonly ICacheService _cacheService;

        public StationController(IStationService stationService,
            ICacheService cacheService)
        {
            _stationService = stationService;
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
                var tracks = _stationService.GetTracks(id);

                var tracksDTO = tracks.Select(Mapper.Map<TrackDTO>);

                return Json(tracksDTO, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.AddExceptionFull(string.Format("Controller: StationController, Action: Get. Args=[id:{0}]", id),
                    ex);

                return Json(new ResponseEntity
                {
                    IsError =  true,
                    Message = "Internal server error.",
                    StatusCode = 500
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #region Methods

       

        #endregion
    }
}