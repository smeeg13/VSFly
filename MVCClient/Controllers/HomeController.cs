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
    public class HomeController : Controller
    {
        private readonly IVSFlyServices _vSFly;

        public HomeController( IVSFlyServices vSFly)
        {
            _vSFly = vSFly;
        }
        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Index(string searchDestination, string searchDeparture)
        {
            var listFlights = await _vSFly.GetFlights();

            if (!string.IsNullOrEmpty(searchDestination))
            {
                listFlights = listFlights.AsQueryable().Where(x => x.Destination.ToLower().Contains(searchDestination));
            }
            if (!string.IsNullOrEmpty(searchDeparture))
            {
                listFlights = listFlights.AsQueryable().Where(x => x.Departure.ToLower().Contains(searchDeparture));
            }

            return View(listFlights);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
