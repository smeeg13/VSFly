using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVCClient.Services;
using MVCClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient.Controllers
{
    public class PassengerController : Controller
    {

        private readonly ILogger<PassengerController> _logger;
        private readonly IVSFlyServices _vSFly;

        public PassengerController(ILogger<PassengerController> logger, IVSFlyServices vSFly)
        {
            _logger = logger;
            _vSFly = vSFly;
        }

        // GET: PassengerController
       [HttpGet]
        public async Task<ActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("UserType") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (!HttpContext.Session.GetString("UserType").Equals("Passenger"))
            {
                return RedirectToAction("Index", "Login");
            }
            var passenger = await _vSFly.GetPassenger((int)HttpContext.Session.GetInt32("PersonId"));

            //Retrieve all tickets for this passenger
            passenger.Tickets = await _vSFly.GetTicketsByPassengerId(passenger.PersonId);

            return View(passenger);
        }

        // GET: PassengerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PassengerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection formCollection)
        {
            PassengerM passenger = new PassengerM();
            if (ModelState.IsValid)
            {

                passenger.FullName = formCollection["FullName"];
                passenger.PassportID = formCollection["PassportID"];
                passenger.Email = formCollection["Email"];
                passenger.Birthday = Convert.ToDateTime(formCollection["Birthday"]);


                var statusCode = _vSFly.CreatePassenger(passenger);
                if (statusCode)
                {
                    //Creation of the Passenger is OK
                    return RedirectToAction("Index");
                }
            }


            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(passenger);
        }


        // GET: PassengerController/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            var p = await _vSFly.GetPassenger(id);

            return View(p);
        }

        // GET: PassengerController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var Passenger = await _vSFly.GetPassenger(id);
            return View(Passenger);
        }

        // POST: PassengerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            PassengerM passenger = new();
            passenger.PersonId = Int32.Parse(collection["PersonId"]);
            passenger.FullName = collection["Fullname"];
            passenger.Email = collection["Email"];
            passenger.Birthday = Convert.ToDateTime(collection["Birthday"]);
            passenger.PassportID =collection["PassportID"];

            var statusCode = _vSFly.UpdatePassenger(passenger);
            if (statusCode)
            {
                //Update of the Passenger is OK

                return RedirectToAction("Details", "Passenger", new { id = passenger.PersonId });
            }

            return RedirectToAction("Edit","Passenger", new { id = passenger.PersonId });
        }

        // GET: PassengerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PassengerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
