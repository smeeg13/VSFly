using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVCClient2.Models;
using MVCWebAPIclient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MVCClient2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            HttpClient client = new HttpClient();
            string baseURI = "https://localhost/44377";

            FlightsClients vsfly = new FlightsClients(baseURI,client);

            ICollection<MVCWebAPIclient.FlightM> listOfFlights = await vsfly.FlightsAllAsync();
            return View(listOfFlights);
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
