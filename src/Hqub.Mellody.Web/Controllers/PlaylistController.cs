using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Hqub.Mellody.Music.Services;
using Hqub.Mellody.Music.Services.Exceptions;
using Hqub.Mellody.Music.Store.Models;
using Hqub.Mellody.Web.Models.Response;

namespace Hqub.Mellody.Web.Controllers
{
    public class PlaylistController : Controller
    {

        public PlaylistController(IPlaylistService playlistService,
            IStationService stationService,
            ICacheService cacheService)
        {
           
        }

        // GET: Radio
        public ActionResult Index()
        {
            return View();
        }
    }
}