using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Extensions
{
    public static class ConverterExtensions
    {

        //Converting models to DB

        public static Models.FlightM ConvertToFlightM(this VSFly.Flight f)
        {
            Models.FlightM fm = new Models.FlightM();
            fm.FlightNo = f.FlightNo;
            fm.Departure = f.Departure;
            fm.Destination = f.Destination;
            fm.Date = f.Date;
            return fm;
        }

        public static VSFly.Flight ConvertToFlight(this Models.FlightM fm)
        {
            VSFly.Flight f = new VSFly.Flight();
            f.FlightNo = fm.FlightNo;
            f.Departure = fm.Departure;
            f.Destination = fm.Destination;
            f.Date = fm.Date;
            return f;
        }

        public static Models.PilotM ConvertToPilotM(this VSFly.Pilot p)
        {
            Models.PilotM pm = new Models.PilotM();
            pm.PersonId = p.PersonId;
            pm.FlightHours = p.FlightHours;
           
            return pm;
        }

        public static VSFly.Pilot ConvertToPilot(this Models.PilotM pm)
        {
            VSFly.Pilot p = new VSFly.Pilot();
            p.PersonId = pm.PersonId;
            p.FlightHours = pm.FlightHours;
            
            return p;
        }

        public static Models.BookingM ConvertToBookingM(this VSFly.Booking b)
        {
            Models.BookingM bm = new Models.BookingM();
            bm.FlightNo = b.FlightNo;
            bm.PassengerID = b.PassengerID;

            return bm;
        }

        public static VSFly.Booking ConvertToBooking(this Models.BookingM bm)
        {
            VSFly.Booking b = new VSFly.Booking();
            b.FlightNo = bm.FlightNo;
            b.PassengerID = bm.PassengerID;

            return b;
        }

        public static VSFly.Passenger ConvertToPassenger(this Models.PassengerM pm)
        {
            VSFly.Passenger p = new VSFly.Passenger();
            p.Birthday = pm.Birthday;
            p.Email = pm.Email;
            p.FullName = pm.FullName;
            p.PersonId = pm.PersonId;
            p.Status = pm.Status;

            return p;
        }
        public static Models.PassengerM ConvertToPassengerM(this VSFly.Passenger p)
        {
            Models.PassengerM pm = new Models.PassengerM();
            pm.Birthday = p.Birthday;
            pm.Email = p.Email;
            pm.FullName = p.FullName;
            pm.PersonId = p.PersonId;
            pm.Status = p.Status;

            return pm;
        }
    }
}
