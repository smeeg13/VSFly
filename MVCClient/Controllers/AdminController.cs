using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCClient.Services;
using MVCClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public async Task<ActionResult> Index(string searchDestination, string searchDeparture, string searchFlightNo, Boolean onlyAvailableFlights)
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
            var flights= await _vSFly.GetAllAdminFlights();
            if (onlyAvailableFlights)
            {
                flights = flights.Where(x => x.FreeSeats > 0);
            }

            admin.Flights = flights;
            // Use LINQ to get list of destination names.
            IQueryable<int> flightsNos = from m in flights.AsQueryable()
                                               orderby m.FlightNo
                                               select m.FlightNo;
            admin.flightsNo = new SelectList(flightsNos);

            if (!string.IsNullOrEmpty(searchDestination))
            {
                admin.Flights = admin.Flights.AsQueryable().Where(x => x.Destination.ToLower().Contains(searchDestination.ToLower()));
            }
            else
            {
                if (!string.IsNullOrEmpty(searchDeparture))
                {
                    admin.Flights = admin.Flights.AsQueryable().Where(x => x.Departure.ToLower().Contains(searchDeparture.ToLower()));
                }
            }
          
            if (!string.IsNullOrEmpty(searchFlightNo))
            {
                admin.Flights = admin.Flights.AsQueryable().Where(x => x.FlightNo == Int32.Parse(searchFlightNo));
            }


            var pilots = await _vSFly.GetPilots();
            admin.Pilots = pilots;
            foreach(PilotAdminM p in admin.Pilots)
            {
                if(p.FlightHours ==null)
                {
                    p.FlightHours = 0;
                }
            }

            var passengers = await _vSFly.GetPassengers();
            admin.Passengers = passengers;

            return View(admin);
        }


        // GET: Admin/DetailsFlight/5
        public async Task<ActionResult> DetailsFlight(int id)
        {
            ViewBag.Message = HttpContext.Session.GetString("UserType");

            var f = await _vSFly.GetAdminFlight(id);
            f.Tickets = (List<Ticket>)await _vSFly.GetTicketsByFlightNo(f.FlightNo);

            return View(f);
        }

        // GET: Admin/EditFlight/5
        [HttpGet]
        public async Task<ActionResult> EditFlight(int id)
        {
            if (!HttpContext.Session.GetString("UserType").Equals("Admin"))
            {
                return RedirectToAction("Index", "Login");
            }
            var Flight = await _vSFly.GetAdminFlight(id);

            return View(Flight);
        }

        // POST: AdminController/EditFlight/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFlight(int id, FlightAdminM flightAdmin)
        {
            if (!HttpContext.Session.GetString("UserType").Equals("Admin"))
            {
                return RedirectToAction("Index", "Login");
            }
            flightAdmin.FlightNo = id;
            if (ModelState.IsValid)
            {
                FlightAdminM flight = new();
                flight.FlightNo = id;
                flight.AirlineName = flightAdmin.AirlineName;
                flight.CopilotId = flightAdmin.CopilotId;
                flight.PilotId = flightAdmin.PilotId;
                flight.Date = flightAdmin.Date;
                flight.Departure = flightAdmin.Departure;
                flight.Destination = flightAdmin.Destination;
                flight.Price = flightAdmin.Price;
                flight.Seat = flightAdmin.Seat;

                var statusCode = _vSFly.UpdateFlight(flight);
                if (statusCode)
                {
                    //Update of the Flight is OK
                    return RedirectToAction("DetailsFlight", "Admin", new { id = flight.FlightNo });
                }
                ModelState.AddModelError(string.Empty, "Something went wrong, Please contact the administration");
                return View("EditFlight", flightAdmin);
            }

            ModelState.AddModelError(string.Empty, "Please control that the info you entered are in the right format");
            return View("EditFlight", flightAdmin);
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
        public ActionResult CreateFlight(FlightAdminM flightAdminM)
        {
            if (!HttpContext.Session.GetString("UserType").Equals("Admin"))
            {
                return RedirectToAction("Index", "Login");
            }
            FlightAdminM f = new();
            if (ModelState.IsValid)
            {

                f.Departure = flightAdminM.Departure;
                f.Destination = flightAdminM.Destination;
                f.Date = flightAdminM.Date;
                f.AirlineName = flightAdminM.AirlineName;
                f.CopilotId = flightAdminM.CopilotId;
                f.PilotId = flightAdminM.PilotId;
                f.Price = flightAdminM.Price;
                f.Seat = flightAdminM.Seat;



                var statusCode = _vSFly.CreateFlight(f);
                if (statusCode)
                {
                    //Creation of the Flight is OK
                
                    return RedirectToAction("Index", "Admin");
                }
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                return View();
            }


            ModelState.AddModelError(string.Empty, "Please control that the info you entered are in the right format");
            return View();

        }
        // GET: Admin/CreateFlight/
        [HttpGet]
        public async Task<ActionResult> DeleteFlight(int id)
        {
            if (!HttpContext.Session.GetString("UserType").Equals("Admin"))
            {
                return RedirectToAction("Index", "Login");
            }
            var flight = await _vSFly.GetAdminFlight(id);
            return View(flight);
        }

        // POST: AdminController/DeleteFlight/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteFlight(int id, FlightAdminM flightAdmin)
        {

            var admin = new Admin();
            var flights = await _vSFly.GetAllAdminFlights();
            admin.Flights = flights;
            var pilots = await _vSFly.GetPilots();
            admin.Pilots = pilots;
            var passengers = await _vSFly.GetPassengers();
            admin.Passengers = passengers;

            Boolean isDeleted = _vSFly.DeleteFlight(id);
            if (isDeleted)
                return RedirectToAction("Index", "Admin", admin);

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View();

        }

        // GET: Admin/CreatePilot/
        public ActionResult CreatePilot()
        {
            if (!HttpContext.Session.GetString("UserType").Equals("Admin"))
                return RedirectToAction("Index", "Login");
            
            return View();
        }

        // POST: Admin/CreatePilot
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePilot(PilotAdminM pilotM)
        {
            PilotAdminM p = new();
            if (ModelState.IsValid)
            {
                p.FullName =pilotM.FullName;
                p.PassportID = pilotM.PassportID;
                p.Salary = pilotM.Salary;
                p.Email = pilotM.Email;
                p.Birthday = pilotM.Birthday;

                var statusCode = _vSFly.CreatePilot(p);
                if (statusCode)
                {
                    //Creation of the Pilot is OK
                    return RedirectToAction("Index", "Admin");
                }
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                return View();
            }
            ModelState.AddModelError(string.Empty, "Please control that the info you entered are in the right format");
            return View();
        }

        // GET: Admin/CreateFlight/
        [HttpGet]
        public async Task<ActionResult> DeletePilot(int id)
        {
            if (!HttpContext.Session.GetString("UserType").Equals("Admin"))
            {
                return RedirectToAction("Index", "Login");
            }
            var pilot = await _vSFly.GetPilot(id);
            return View(pilot);
        }

        // POST: AdminController/DeleteFlight/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeletePilot(int id, PilotAdminM pilotAdmin)
        {

            var admin = new Admin();
            var flights = await _vSFly.GetAllAdminFlights();
            admin.Flights = flights;
            var pilots = await _vSFly.GetPilots();
            admin.Pilots = pilots;
            var passengers = await _vSFly.GetPassengers();
            admin.Passengers = passengers;

            Boolean isDeleted = _vSFly.DeletePilot(id);
            if (isDeleted)
                return RedirectToAction("Index", "Admin", admin);

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View();

        }


        // GET: Admin/ManageAdmin/
        [HttpGet]
        [HttpPost]
        public async  Task<ActionResult> ManageAdmin(string PassportIdChoosed, string fullname)
        {
            if (!HttpContext.Session.GetString("UserType").Equals("Admin"))
            {
                return RedirectToAction("Index", "Login");
            }

            PassengerAdmin passengerAdmin = new();
            var passengers = await _vSFly.GetPassengers();
            passengerAdmin.Passengers = passengers;
            IQueryable<string> passportQuery = from m in passengers.AsQueryable()
                                               orderby m.PassportID
                                               select m.PassportID;
            passengerAdmin.PassportIdList = new SelectList(passportQuery);
            if (!string.IsNullOrEmpty(PassportIdChoosed))
            {
                passengerAdmin.Passengers = passengers.Where(x => x.PassportID ==PassportIdChoosed);
            }
            if (!string.IsNullOrEmpty(fullname))
            {
                passengerAdmin.Passengers = passengers.Where(x => x.FullName.ToLower().Contains(fullname));
            }

            return View(passengerAdmin);
        }

        // POST: AdminController/Admin/ChangeStatusEdit/5
        [HttpGet]
        [HttpPost]
        public async Task<ActionResult> ChangeStatus(int id, string status)
        {
            PassengerM passenger = new();
            PassengerAdmin passengerAdmin = new();
            var passengers = await _vSFly.GetPassengers();
                    passengerAdmin.Passengers = passengers;
                    IQueryable<string> passportQuery = from m in passengers.AsQueryable()
                                                       orderby m.PassportID
                                                       select m.PassportID;
            passengerAdmin.PassportIdList = new SelectList(passportQuery);
            passenger = await _vSFly.GetPassenger(id);
            if (passenger != null)
            {
                if (status.Equals("Passenger"))
                    passenger.Status = "Admin";
                else
                    passenger.Status = "Passenger";
                
                var statusCode = _vSFly.UpdatePassenger(passenger);
                if (statusCode)
                {
                    //Update of the Passenger is OK
                    return View("ManageAdmin", passengerAdmin);
                }
            }
            ModelState.AddModelError(string.Empty, "Something went wrong, Please contact the administration");
            return View("ManageAdmin", passengerAdmin);
        }


        // GET: Admin/DeletePassenger/
        [HttpGet]
        public async Task<ActionResult> DeletePassenger(int id)
        {
            if (!HttpContext.Session.GetString("UserType").Equals("Admin"))
            {
                return RedirectToAction("Index", "Login");
            }
            var Passenger = await _vSFly.GetPassenger(id);
            return View(Passenger);
        }

        // POST: AdminController/DeleteFlight/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeletePassenger(int id, PassengerM passenger)
        {

            var admin = new Admin();
            var flights = await _vSFly.GetAllAdminFlights();
            admin.Flights = flights;
            var pilots = await _vSFly.GetPilots();
            admin.Pilots = pilots;
            var passengers = await _vSFly.GetPassengers();
            admin.Passengers = passengers;

            Boolean isDeleted = _vSFly.DeletePassenger(id);
            if (isDeleted)
                return RedirectToAction("Index", "Admin", admin);

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View();

        }

    }
}
