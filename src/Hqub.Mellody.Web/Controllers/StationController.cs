using System;
using System.Web.Mvc;
using Hqub.Mellody.Poco;

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

            return View(new StationDTO
            {
                Id = guid
            });
        }

        #region Methods

       

        #endregion
    }
}