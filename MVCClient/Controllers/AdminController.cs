using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCClient.Services;
using MVCClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient.Controllers
{
    public class AdminController : Controller
    {

        private readonly IVSFlyServices _vSFly;

        public AdminController(IVSFlyServices vSFly)
        {
            _vSFly = vSFly;
        }


        // GET: AdminController
        [HttpGet]
        [HttpPost]
        public async Task<ActionResult> Index(string searchDestination, string searchDeparture, int searchFlightNo)
        {
            if (HttpContext.Session.GetInt32("PersonId") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (!HttpContext.Session.GetString("UserType").Equals("Admin"))
            {
                return RedirectToAction("Index", "Login");
            }
            var admin = new Admin();
            var flights = await _vSFly.GetAllAdminFlights();

            admin.Flights = flights;

            if (!string.IsNullOrEmpty(searchDestination))
            {
                admin.Flights = admin.Flights.AsQueryable().Where(x => x.Destination.Contains(searchDestination));
            }
            if (!string.IsNullOrEmpty(searchDeparture))
            {
                admin.Flights = admin.Flights.AsQueryable().Where(x => x.Departure.Contains(searchDeparture));
            }
            if (!string.IsNullOrEmpty(searchDeparture))
            {
                admin.Flights = admin.Flights.AsQueryable().Where(x => x.FlightNo == searchFlightNo);
            }

            var pilots = await _vSFly.GetPilots();
            admin.Pilots = pilots;

            var passengers = await _vSFly.GetPassengers();
            admin.Passengers = passengers;

            return View(admin);
        }


        // GET: Admin/DetailsFlight/5
        public async Task<ActionResult> DetailsFlight(int id)
        {
            ViewBag.Message = HttpContext.Session.GetString("UserType");

            var f = await _vSFly.GetAdminFlight(id);

            return View(f);
        }

        // GET: Admin/CreateFlight/
        public ActionResult CreateFlight()
        {
            if (!HttpContext.Session.GetString("UserType").Equals("Admin"))
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        // POST: Admin/CreatFlight
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFlight(IFormCollection formCollection)
        {
            if (!HttpContext.Session.GetString("UserType").Equals("Admin"))
            {
                return RedirectToAction("Index", "Login");
            }
            FlightAdminM f = new();
            if (ModelState.IsValid)
            {

                f.Departure = formCollection["Departure"];
                f.Destination = formCollection["Destination"];
                f.Date = Convert.ToDateTime(formCollection["Date"]);
                f.AirlineName = formCollection["AirlineName"];
                f.CopilotId = Int32.Parse(formCollection["CopilotId"]);
                f.PilotId = Int32.Parse(formCollection["PilotId"]);
                f.Price = Double.Parse(formCollection["Price"]);
                f.Seat = Int32.Parse(formCollection["Seat"]);



                var statusCode = _vSFly.CreateFlight(f);
                if (statusCode)
                {
                    //Creation of the Flight is OK
                
                    return RedirectToAction("Index", "Admin");
                }
            }


            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(f);

        }

        // GET: Admin/CreatePilot/
        public ActionResult CreatePilot()
        {
            if (!HttpContext.Session.GetString("UserType").Equals("Admin"))
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        // POST: Admin/CreatePilot
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePilot(IFormCollection formCollection)
        {
            PilotAdminM p = new();
            if (ModelState.IsValid)
            {

                p.FullName = formCollection["FullName"];
                p.PassportID = formCollection["PassportID"];
                p.Salary = Double.Parse(formCollection["Salary"]);
                p.Email = formCollection["Email"];
                p.Birthday = Convert.ToDateTime(formCollection["Birthday"]);
              

                var statusCode = _vSFly.CreatePilot(p);
                if (statusCode)
                {
                    //Creation of the Pilot is OK

                    return RedirectToAction("Index", "Admin");
                }
            }


            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(p);

        }

        // GET: Admin/EditFlight/5
        public async Task<ActionResult> EditFlight(int id)
        {
            if (!HttpContext.Session.GetString("UserType").Equals("Admin"))
            {
                return RedirectToAction("Index", "Login");
            }
            var Flight = await _vSFly.GetAdminFlight(id);

            return View(Flight);
        }

        // POST: BookingController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            if (!HttpContext.Session.GetString("UserType").Equals("Admin"))
            {
                return RedirectToAction("Index", "Login");
            }
            FlightAdminM flight = new();
            flight.AirlineName = collection["AirlineName"];
            flight.CopilotId = Int32.Parse(collection["CopilotId"]);
            flight.PilotId = Int32.Parse(collection["PilotId"]);
            flight.Date = Convert.ToDateTime(collection["Date"]);
            flight.Departure = collection["Departure"];
            flight.Destination = collection["Destination"];
            flight.Price = Double.Parse(collection["Price"]);
            flight.Seat = Int32.Parse(collection["Seat"]);

            var statusCode = _vSFly.UpdateFlight(flight);
            if (statusCode)
            {
                //Update of the Passenger is OK

                return RedirectToAction("DetailsFlight", "Admin", new { id = flight.FlightNo });
            }


            return RedirectToAction("EditFlight", "Admin", new { id = flight.FlightNo });
        }

        // GET: BookingController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BookingController/Delete/5
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
