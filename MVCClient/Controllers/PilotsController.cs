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

            if (HttpContext.Session.GetInt32("PersonId") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (!HttpContext.Session.GetString("UserType").Equals("Pilot"))
            {
                return RedirectToAction("Index", "Login");
            }


            var Pilot = await _vSFly.GetPilot((int)HttpContext.Session.GetInt32("PersonId"));

            Pilot.FlightsToPilot = await _vSFly.GetFlightsByPilotId(Pilot.PersonId);
            Pilot.FlightsToCoPilot = await _vSFly.GetFlightsByCoPilotId(Pilot.PersonId);


            return View(Pilot);
        }

        // GET: PilotController/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            var pilot = await _vSFly.GetPilot(id);

            return View(pilot);
        }

        // GET: PilotController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var pilot = await _vSFly.GetPilot(id);
            return View(pilot);
        }

        // POST: PilotController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PilotAdminM pilotM)
        {
            pilotM.PersonId = id;
            if (ModelState.IsValid)
            {
                PilotAdminM pilot = new();
                pilot.PersonId = id;
                pilot.FullName = pilotM.FullName;
                pilot.Email = pilotM.Email;
                pilot.Birthday = pilotM.Birthday;
                pilot.PassportID = pilotM.PassportID;

                var statusCode = _vSFly.UpdatePilot(pilot);
                if (statusCode)
                {
                    //Update of the Passenger is OK

                    return RedirectToAction("Details", "Pilots", new { id = pilot.PersonId });
                }
                ModelState.AddModelError(string.Empty, "Something went wrong, Please contact the administration");

                return RedirectToAction("Edit", pilotM);
            }
            ModelState.AddModelError(string.Empty, "Please control that the info you entered are in the right format");

            return View("Edit", pilotM);
        }

    }
}
