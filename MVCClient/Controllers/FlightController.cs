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

        public async Task<ActionResult> BookFlight(BookFlight bookFlight)
        {
            List<BookingM> bookingMs = new List<BookingM>();
             var passengers = await _vSFly.GetPassengers();

            //Create passenger into db IF Dont exists
            for (int i = 0; i<passengers.Count(); i++)
            {
               
                    if (passengers.ElementAt(i).Email.Equals(bookFlight.Passenger.Email))
                    {
                        //Passenger already exist
                        bookFlight.Passenger.PersonId = passengers.ElementAt(i).PersonId;
                    }
                    else
                    {
                        //Must create the passenger
                        var statusCode = _vSFly.CreatePassenger(bookFlight.Passenger);
                        if (statusCode)
                        {
                            //Creation of the Passenger is OK
                            //assign the new passenger id into bookflight.passengers
                            PassengerM passengerM = await _vSFly.GetPassengerByPassportID(bookFlight.Passenger.PassportID);
                            bookFlight.Passenger.PersonId = passengerM.PersonId;
                        }
                       
                    }
                                  
                
            }
            

                //Create new booking for each passenger
                BookingM bookingM = new BookingM();
                bookingM.FlightNo = bookFlight.FlightNo;
                bookingM.SalePrice = bookFlight.SalePrice;
                bookingM.PassengerID = bookFlight.Passenger.PersonId;

            var statusCode2 = _vSFly.CreateBooking(bookingM);
            if (statusCode2)
            {
                //Creation Booking ok
                //Display Booking View
                bookFlight.Booking = bookingM;
                return View(bookFlight);

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
