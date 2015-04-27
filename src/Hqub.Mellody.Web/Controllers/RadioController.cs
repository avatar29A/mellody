using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hqub.Mellody.Web.Controllers
{
    public class RadioController : Controller
    {
        // GET: Radio
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Index(Models.PrepareRadioModel model)
        {
            return Json(new Models.Response.RadioCreatedResponse());
        }

        public ActionResult Station()
        {
            return View();
        }
    }
}