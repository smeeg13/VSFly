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
            if (HttpContext.Session.GetInt32("UserType") == null)
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

            return View(admin);
        }


        // GET: Admin/DetailsFlight/5
        public async Task<ActionResult> DetailsFlight(int flightNo)
        {
            var f = await _vSFly.GetAdminFlight(flightNo);

            return View(f);
        }

        // GET: Admin/CreateFlight/
        public ActionResult CreateFlight()
        {
            return View();
        }

        // POST: Admin/CreatFlight
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFlight(IFormCollection formCollection)
        {
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
        public ActionResult EditFlight(int id)
        {
            return View();
        }

        // POST: BookingController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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
