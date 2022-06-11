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
        public async Task<ActionResult> Details(int id, int NbPassengers)
        {
            var flight = await _vSFly.GetFlight(id);

            var bookFlight = new BookFlight();
            bookFlight.FlightNo = flight.FlightNo;
            bookFlight.Departure = flight.Departure;
            bookFlight.Destination = flight.Destination;
            bookFlight.Date = flight.Date;
            bookFlight.SalePrice = flight.SalePrice;
            if (NbPassengers != 1)
            {
                NbPassengers = 1;
            }
            bookFlight.NbPassengers = 1;
            //bookFlight.Passengers = new List<PassengerM>(NbPassengers);
            //for(int i =0; i < NbPassengers; i++)
            //{
            //    bookFlight.Passengers.Add(new PassengerM());
            //}
            
            return View(bookFlight);
        }

        // GET: FlightController/Details/5
        [HttpGet]
        public async Task<ActionResult> BaseDetails(int id)
        {
            var flight = await _vSFly.GetFlight(id);

           

            return View(flight);
        }


        
        [HttpGet]
        [HttpPost]
        public async Task<ActionResult> BookFlight(BookFlight bookFlight)
        {
            bookFlight.Passenger = new PassengerM();
            bookFlight.Passenger.FullName = bookFlight.FullName;
            bookFlight.Passenger.Email = bookFlight.Email;
            bookFlight.Passenger.PassportID = bookFlight.PassportID;
            bookFlight.Passenger.Birthday = bookFlight.Birthday;
            
            if (ModelState.IsValid)
            {
                ViewBag.Fullname = bookFlight.FullName;
                ViewBag.Email = bookFlight.Email;
                ViewBag.PassportID = bookFlight.PassportID;
                ViewBag.Birthday = bookFlight.Birthday;

                var passengers = await _vSFly.GetPassengers();
                Boolean AlreadyExist = false;
                //Create passenger into db IF Dont exists
                for (int i = 0; i < passengers.Count(); i++)
                {
                    if (bookFlight.PassportID == passengers.ElementAt(i).PassportID)
                    {
                        AlreadyExist = true;
                        //Passenger already exist
                        bookFlight.Passenger = passengers.ElementAt(i);
                        ViewBag.AlreadyExist = true;

                       
                    }
                }

                if (AlreadyExist)
                {
                   
                    //Check if this passenger already have a booking for this flight
                        var bookingsAll = await _vSFly.GetBookingsByPassengerId(bookFlight.Passenger.PersonId);

                       foreach(BookingM bookingM in bookingsAll)
                    {
                        if(bookFlight.FlightNo == bookingM.FlightNo)
                        {
                            //Passenger already have a ticket for this flight
                            ModelState.AddModelError(string.Empty, "You already have a ticket for this flight ! ");

                            return View("Details", bookFlight);
                        }
                    }
                       
                       //Go to confirmation of ticket
                        return View(bookFlight);
                }
                else
                {
                    //Must create the passenger
                    bookFlight.Passenger.Status = "Passenger";
                    var statusCode = _vSFly.CreatePassenger(bookFlight.Passenger);
                    if (statusCode)
                    {
                        //Creation of the Passenger is OK
                        //assign the new passenger id into bookflight.passengers
                        PassengerM passengerM = await _vSFly.GetPassengerByPassportID(bookFlight.Passenger.PassportID);
                        bookFlight.Passenger = passengerM;
                        ViewBag.AlreadyExist = false;
                        return View(bookFlight);

                    }
                }
                ModelState.AddModelError(string.Empty, "there was a problem, contact administrators ! ");

                return View();

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please control that the info you entered are in the right format");

                return View("Details", bookFlight);
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

           
        }


        // POST: PassengerController/UpdateFromBooking/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateFromBooking(int id, BookFlight bookFlight)
        {

            bookFlight.Passenger.PersonId = id; 
            var statusCode = _vSFly.UpdatePassenger(bookFlight.Passenger);
            if (statusCode)
            {
                //Update of the Passenger is OK
                ViewBag.Fullname = bookFlight.Passenger.FullName;
                ViewBag.Email = bookFlight.Passenger.Email;
                ViewBag.PassportID = bookFlight.Passenger.PassportID;
                ViewBag.Birthday = bookFlight.Passenger.Birthday;
                ViewBag.AlreadyExist = true;
                ModelState.AddModelError(string.Empty, "Modification Has been Made");
                return View("BookFlight", bookFlight);
            }
            ModelState.AddModelError(string.Empty, "Something went wrong, please contact the administrator");


            return View();
        }



        [HttpGet]
        [HttpPost]
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

                //HttpContext.Session.SetString("UserType", "Passenger");
                //HttpContext.Session.SetInt32("PersonId", bookFlight.Booking.PassengerID);
                return View(bookFlight);

            }
            ModelState.AddModelError(string.Empty, "Something went wrong, please contact the administrator");


            return View();
        }
    }
}
