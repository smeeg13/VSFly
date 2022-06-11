using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Extensions
{
    public static class ConverterExtensions
    {

       
        public static Models.Ticket GenerateTicket(this VSFly.Booking b )
        {

            Models.Ticket t = new();
            t.PassengerId = b.PassengerID;
            t.FlightNo = b.FlightNo;
            t.FullName = b.Passenger.FullName;
            t.Departure = b.Flight.Departure;
            t.Destination = b.Flight.Destination;
            t.Date = b.Flight.Date;
            t.SalePrice = b.SalePrice;

            return t;
        }
        
        
        //Converting models to DB
        
        public static Models.FlightM ConvertToFlightM(this VSFly.Flight f)
        {
            Models.FlightM fm = new Models.FlightM();
            fm.FlightNo = f.FlightNo;
            fm.Departure = f.Departure;
            fm.Destination = f.Destination;
            fm.Date = f.Date;
            fm.Price = f.Price;
            return fm;
        }

        public static VSFly.Flight ConvertToFlight(this Models.FlightM fm)
        {
            VSFly.Flight f = new VSFly.Flight();
            f.FlightNo = fm.FlightNo;
            f.Departure = fm.Departure;
            f.Destination = fm.Destination;
            f.Date = fm.Date;
            f.Price = fm.Price;
            return f;
        }
        //AdminConverter Flight
        public static Models.FlightAdminM ConvertToFlightAdminM(this VSFly.Flight f)
        {
            Models.FlightAdminM fm = new Models.FlightAdminM();
            fm.FlightNo = f.FlightNo;
            fm.Departure = f.Departure;
            fm.Destination = f.Destination;
            fm.Date = f.Date;
            fm.Price = f.Price;
            
            fm.PilotId = f.PilotId;
  
            fm.CopilotId = f.CopilotId;
            fm.Seat = f.Seat;
            fm.FreeSeats = f.FreeSeats;
            fm.AirlineName = f.AirlineName;
            return fm;
        }
        public static VSFly.Flight ConvertToFlightFromAdmin(this Models.FlightAdminM fAdmin)
        {
            VSFly.Flight f = new VSFly.Flight();
            f.FlightNo = fAdmin.FlightNo;
            f.Departure = fAdmin.Departure;
            f.Destination = fAdmin.Destination;
            f.Date = fAdmin.Date;
            f.Price = fAdmin.Price;
            
            f.PilotId = fAdmin.PilotId;
           
            f.CopilotId = fAdmin.CopilotId;
            f.Seat = fAdmin.Seat;
            f.FreeSeats = fAdmin.FreeSeats;
            f.AirlineName = fAdmin.AirlineName;
            return f;
        }

        public static Models.PilotM ConvertToPilotM(this VSFly.Pilot p)
        {
            Models.PilotM pm = new Models.PilotM();
            pm.PersonId = p.PersonId;
            pm.FlightHours = p.FlightHours;
            if (pm.FlightHours == null)
                pm.FlightHours = 0;
           
            return pm;
        }

        public static VSFly.Pilot ConvertToPilot(this Models.PilotM pm)
        {
            VSFly.Pilot p = new VSFly.Pilot();
            p.PersonId = pm.PersonId;
            p.FlightHours = pm.FlightHours;
            if (p.FlightHours == null)
                p.FlightHours = 0;

            return p;
        }


        //AdminConverter Pilot
        public static Models.PilotAdminM ConvertToPilotAdminM(this VSFly.Pilot p)
        {
            Models.PilotAdminM pm = new Models.PilotAdminM();
            pm.PersonId = p.PersonId;
            pm.FlightHours = p.FlightHours;
            if (pm.FlightHours == null)
                pm.FlightHours = 0;
            pm.Birthday = p.Birthday;
            pm.Email = p.Email;
            pm.FullName = p.FullName;
            pm.PassportID = p.PassportID;
            pm.Salary = p.Salary;

            return pm;
        }

        public static VSFly.Pilot ConvertToPilotFromAdmin(this Models.PilotAdminM pm)
        {
            VSFly.Pilot p = new VSFly.Pilot();
            p.PersonId = pm.PersonId;
            p.FlightHours = pm.FlightHours;
            if (p.FlightHours == null)
                p.FlightHours = 0;
            p.Birthday = pm.Birthday;
            p.Email = pm.Email;
            p.FullName = pm.FullName;
            p.PassportID = pm.PassportID;
            p.Salary = pm.Salary;

            return p;
        }

     

        public static Models.BookingM ConvertToBookingM(this VSFly.Booking b)
        {
            Models.BookingM bm = new Models.BookingM();
            bm.FlightNo = b.FlightNo;
            bm.PassengerID = b.PassengerID;
            bm.SalePrice = b.SalePrice;
            return bm;
        }

        public static VSFly.Booking ConvertToBooking(this Models.BookingM bm)
        {
            VSFly.Booking b = new VSFly.Booking();
            b.FlightNo = bm.FlightNo;
            b.PassengerID = bm.PassengerID;
            b.SalePrice = bm.SalePrice;

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
          
            p.PassportID = pm.PassportID;


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

            pm.PassportID = p.PassportID;

            return pm;
        }
    }
}
