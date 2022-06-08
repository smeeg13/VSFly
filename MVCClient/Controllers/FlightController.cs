using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCClient.Models;
using MVCClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient.Controllers
{
    public class FlightController : Controller


    {
        private readonly IVSFlyServices _vSFly;

        public FlightController(IVSFlyServices vSFly)
        {
            _vSFly = vSFly;
        }


        // GET: FlightController
        public ActionResult Index()
        {
            return View();
        }

        // GET: FlightController/Details/5
        [HttpGet]
        [HttpPost]
        public async Task<ActionResult> Details(int id, int NbPassengers)
        {
            var flight = await _vSFly.GetFlight(id);

            var bookFlight = new BookFlight();
            bookFlight.FlightNo = flight.FlightNo;
            bookFlight.Departure = flight.Departure;
            bookFlight.Destination = flight.Destination;
            bookFlight.Date = flight.Date;
            bookFlight.SalePrice = flight.SalePrice;
            if (NbPassengers < 1)
            {
                NbPassengers = 1;
            }
            bookFlight.NbPassengers = NbPassengers;
            bookFlight.Passengers = new List<PassengerM>(NbPassengers);
            for(int i =0; i < NbPassengers; i++)
            {
                bookFlight.Passengers.Add(new PassengerM());
            }
            
            return View(bookFlight);
        }

        // GET: FlightController/Details/5
        [HttpGet]
        public async Task<ActionResult> BaseDetails(int id)
        {
            var flight = await _vSFly.GetFlight(id);

           

            return View(flight);
        }


        
        public async Task<ActionResult> BookFlight(BookFlight bookFlight)
        {
            ViewBag.Fullname = bookFlight.Passenger.FullName;
            ViewBag.Email = bookFlight.Passenger.Email;
            ViewBag.PassportID = bookFlight.Passenger.PassportID;
            ViewBag.Birthday = bookFlight.Passenger.Birthday;

            List<BookingM> bookingMs = new List<BookingM>();
             var passengers = await _vSFly.GetPassengers();
            Boolean AlreadyExist = false;
            //Create passenger into db IF Dont exists
            for (int i = 0; i<passengers.Count(); i++)
            {
                    if (bookFlight.Passenger.PassportID == passengers.ElementAt(i).PassportID)
                    {
                        AlreadyExist = true;
                        //Passenger already exist
                        bookFlight.Passenger = passengers.ElementAt(i);
                    ViewBag.AlreadyExist = true;

                }
            }

            if (!AlreadyExist)
            {
                //Must create the passenger
                var statusCode = _vSFly.CreatePassenger(bookFlight.Passenger);
                if (statusCode)
                {
                    //Creation of the Passenger is OK
                    //assign the new passenger id into bookflight.passengers
                    PassengerM passengerM = await _vSFly.GetPassengerByPassportID(bookFlight.Passenger.PassportID);
                    bookFlight.Passenger = passengerM;
                    ViewBag.AlreadyExist = false;

                }
            }
            

              

            ////Create passenger into db IF Dont exists
            //for (int i = 0; i < passengers.Count(); i++)
            //{
            //    for (int j = 0; j < bookFlight.NbPassengers; j++)
            //    {
            //        if (passengers.ElementAt(i).Email.Equals(bookFlight.Passengers.ElementAt(j).Email))
            //        {
            //            //Passenger already exist
            //            bookFlight.Passengers.ElementAt(j).PersonId = passengers.ElementAt(i).PersonId;
            //        }
            //        else
            //        {
            //            //Must create the passenger
            //            var statusCode = _vSFly.CreatePassenger(bookFlight.Passengers.ElementAt(j));
            //            if (statusCode)
            //            {
            //                //Creation of the Passenger is OK
            //                //assign the new passenger id into bookflight.passengers
            //                PassengerM passengerM = await _vSFly.GetPassengerByPassportID(bookFlight.Passengers.ElementAt(j).PassportID);
            //                bookFlight.Passengers.ElementAt(j).PersonId = passengerM.PersonId;
            //            }

            //        }
            //    }

            //}

            //foreach (PassengerM p in bookFlight.Passengers)
            //{

            //    //Create new booking for each passenger
            //    BookingM bookingM = new BookingM();
            //    bookingM.FlightNo = bookFlight.FlightNo;
            //    bookingM.SalePrice = bookFlight.SalePrice;
            //    bookingM.PassengerID = p.PersonId;
            //    bookingMs.Add(bookingM);
            //}

            //Display la réservation

            return View(bookFlight);
        }


        // POST: PassengerController/UpdateFromBooking/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateFromBooking(int id, IFormCollection collection)
        {

            BookFlight book = new();
            book.FlightNo = Int32.Parse(collection["FlightNo"]);
            book.Destination = collection["Destination"];
            book.Departure = collection["Departure"];
            book.Date = Convert.ToDateTime(collection["Date"]);
            book.NbPassengers = 1;
            book.Price = Double.Parse(collection["Price"]);
            book.SalePrice = Double.Parse(collection["SalePrice"]);
            PassengerM passenger = new();
            passenger.PersonId = id;
            passenger.FullName = collection["Fullname"];
            passenger.Email = collection["Email"];
            passenger.Birthday = Convert.ToDateTime(collection["Birthday"]);
            passenger.PassportID = collection["PassportID"];

            var statusCode = _vSFly.UpdatePassenger(passenger);
            if (statusCode)
            {
                //Update of the Passenger is OK
                ViewBag.Fullname = passenger.FullName;
                ViewBag.Email = passenger.Email;
                ViewBag.PassportID = passenger.PassportID;
                ViewBag.Birthday = passenger.Birthday;
                ViewBag.AlreadyExist = true;
                book.Passenger = passenger;
                ModelState.AddModelError(string.Empty, "Modification Has been Made");
                return View("BookFlight", book);
            }
            ModelState.AddModelError(string.Empty, "Something went wrong, please contact the administrator");


            return View("BookFlight", book);
        }




        public async Task<ActionResult> ConfirmBooking(BookFlight bookFlight)
        {
            //Create new booking for each passenger
            BookingM bookingM = new BookingM();
            bookingM.FlightNo = bookFlight.FlightNo;
            bookingM.SalePrice = bookFlight.SalePrice;
            bookingM.PassengerID = bookFlight.Passenger.PersonId;

            var statusCode2 = _vSFly.CreateBooking(bookingM);
            if (statusCode2)
            {
                //Creation Booking ok
                //Get back the booking created
                BookingM bookCreated = await _vSFly.GetSpecificBooking(bookFlight.FlightNo, bookFlight.Passenger.PersonId);
                //Display Booking View
                bookFlight.Booking = bookCreated;
                return View(bookFlight);

            }
            return View();
        }

            // GET: FlightController/Create
            public ActionResult Create()
        {
            return View();
        }

        // POST: FlightController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: FlightController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FlightController/Edit/5
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

        // GET: FlightController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FlightController/Delete/5
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
