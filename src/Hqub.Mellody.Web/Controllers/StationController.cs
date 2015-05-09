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
            var guid = StringToGuid(id);

            if (guid == Guid.Empty)
                return RedirectToAction("Index");

            return View(new StationModel
            {
                StationId = id
            });
        }

        #region Methods

        private Guid StringToGuid(string val)
        {
            Guid guidId;

            if (!string.IsNullOrEmpty(val) && Guid.TryParse(val, out guidId))
            {
                return guidId;
            }

            return Guid.Empty;
        }

        #endregion
    }
}