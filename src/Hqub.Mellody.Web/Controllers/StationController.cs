using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hqub.Mellody.Web.Models.Radio;

namespace Hqub.Mellody.Web.Controllers
{
    public class StationController : Controller
    {
        [HttpGet]
        public ActionResult Index(string id)
        {
            var guid = Music.Helpers.StationHelper.StringToGuid(id);
            if (guid == Guid.Empty)
                return RedirectToAction("Index", "Playlist");

            return View(new StationModel
            {
                StationId = id
            });
        }

        #region Methods

       

        #endregion
    }
}