using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVCClient.Models;
using MVCClient.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient.Controllers
{
    public class PilotsController : Controller
    {
        private readonly ILogger<PilotsController> _logger;
        private readonly IVSFlyServices _vSFly;

        public PilotsController(ILogger<PilotsController> logger, IVSFlyServices vSFly)
        {
            _vSFly = vSFly;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {

            if (HttpContext.Session.GetInt32("UserType") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (!HttpContext.Session.GetString("UserType").Equals("Pilot"))
            {
                return RedirectToAction("Index", "Login");
            }


            var listPilots = await _vSFly.GetPilots();
            return View(listPilots);
        }


    }
}
