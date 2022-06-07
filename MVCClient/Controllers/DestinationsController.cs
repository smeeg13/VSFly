using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCClient.Models;
using MVCClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient.Controllers
{
    public class DestinationsController : Controller
    {

        private readonly IVSFlyServices _vSFly;

        public DestinationsController(IVSFlyServices vSFly)
        {

            _vSFly = vSFly;
        }

        // GET: DestinationsController
        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Index(string NameChoosed)
        {

            var destination = await _vSFly.GetAllDestinations();
            var destNames = await _vSFly.GetAllDestinations(); // Use LINQ to get list of clients.
            IQueryable<string> destNameQuery = from m in destNames.AsQueryable()
                                               orderby m.DestinationName
                                               select m.DestinationName;

            Destinations destView = new Destinations();
            destView.DestinationsAll = destination;
            destView.Names = new SelectList(destNameQuery);


            if (!string.IsNullOrEmpty(NameChoosed))
            {
                destView.DestinationsAll = destination.Where(x => x.DestinationName.Contains(NameChoosed));
            }

            return View(destView);
        }

        // GET: DestinationsController/Details/5
        [HttpGet("Details/{destinationName}")]
        public async Task<ActionResult> Details(string destinationName)
        {
            Destination destination = new();
            var dests = await _vSFly.GetAllDestinations();
            foreach(Destination d in dests)
            {
                if (d.DestinationName.Equals(destinationName))
                {
                    destination = d;
                }
            }
            var flights =await _vSFly.GetFlightsForDestination( destinationName);

            destination.Flights = (List<FlightAdminM>)flights;
            return View(destination);
        }

      
    }
}
