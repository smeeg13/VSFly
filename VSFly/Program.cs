using System;
using System.Collections.Generic;
using System.Linq;

namespace VSFly
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello from VFLy!");

            var context = new VsflyContext();
            var e = context.Database.EnsureCreated();

            if (e)
            {
                Console.Write("DB created");
            }
            else
            {
                Console.Write("DB already exists");

            }
            Console.Write("Done");


            //Add
            //Pilot p = new Pilot() { FlightHours = 10, LicenseDate = DateTime.Today, Birthday = DateTime.Today, Email = "meg@gmail.com", FullName = "Meg Solliard", PassportNumber = "NJ908ZTG8", Salary = 10000 };
            //context.Pilots.Add(p);
            //context.SaveChanges();
            

            //Flight flight1 = new Flight() { AirlineName = "EasyJet", CopilotId = 1, Date = DateTime.Today, Departure = "Prague", Destination = "Dubin", FreeSeats = 500, NonSmokingFlight = false, PilotId = 2, Price = 160.0, Seat = 500, Timestamp = "1h", Utilization = true, Strikebound = false };
            //context.Flights.Add(flight1);
            //context.SaveChanges();

            //Flight flight2 = new Flight() { AirlineName = "Air France", CopilotId = 1, Date = DateTime.Today, Departure = "Amsterdam", Destination = "Zurich", FreeSeats = 500, NonSmokingFlight = false, PilotId = 2, Price = 160.0, Seat = 500, Timestamp = "1h", Utilization = true, Strikebound = false };
            //context.Flights.Add(flight2);
            //context.SaveChanges();

            //Flight flight3 = new Flight() { AirlineName = "	Swiss International Air Lines", CopilotId = 1, Date = DateTime.Today, Departure = "Bern", Destination = "Oslo", FreeSeats = 500, NonSmokingFlight = false, PilotId = 2, Price = 160.0, Seat = 500, Timestamp = "1h", Utilization = true, Strikebound = false };
            //context.Flights.Add(flight3);
            //context.SaveChanges();

            //Console.Write("Add Done");

            //var pilotList = context.Pilots.ToList<Pilot>();
            foreach(Flight f in context.Flights)
            {
                Console.WriteLine("Date: {0}, Destnation : {1}, Seats {2}", f.Date, f.Destination, f.Seat);
            }
            var flightsToPorto = context.Flights.Where(f => f.Departure == "Bern" && f.Seat>100).ToList<Flight>();

            var flightToPortoQ2 = from Flight in context.Flights
                                 where Flight.Departure == "Porto" && Flight.Seat > 100
                                 select Flight;

            var flightToPorto2 = flightToPortoQ2.ToList();

            foreach(Flight f in flightToPorto2)
            {
                Console.WriteLine("Date: {0}, Departure : {1}, Seats {2}", f.Date, f.Departure, f.Seat);
            }
            var passengerJean = context.Passengers.Where(p => p.FullName == "Jean Dutrond").FirstOrDefault<Passenger>();
            
                      
            var bookingsJean = context.Bookings.Where(b => b.Passenger == passengerJean).ToList<Booking>();
            var flightJean = new List<Flight>();
            var pilotJean = new List<Pilot>();
            foreach (Booking b in bookingsJean)
            {
                flightJean = context.Flights.Where(f => f.FlightNo == b.FlightNo).ToList<Flight>();
            }
            foreach(Flight f in flightJean)
            {
                pilotJean = context.Pilots.Where(p => p.PersonId == f.PilotId).ToList<Pilot>();
            }

            foreach(Pilot p in pilotJean)
            {
                Console.WriteLine("Fullname: {0}", p.FullName);
            }
            //var pilotFromJeanDutrond = context.Pilots.Where(context.Flights.Where(context.Bookings.Where(context.Passengers.Where(p => p.FullName == "Jean Dutrond"))));

            //Booking b1 = new Booking() { Flight = flightToPorto2[0], Passenger = new Passenger() { Birthday = DateTime.Today, Email = "crist@gmail.com", FullName = "Cristiana Lima", Status = "occupe" } };

            //Passenger p1 = new Passenger() { Status = "maried", FullName = "Jean Dutrond", Email = "jd@gmail.com", Birthday = DateTime.Today };

            //Booking b2 = new Booking() { Flight = flightsToPorto[0], Passenger = p1 };
            //context.Passengers.Add(p1);
            //context.Bookings.Add(b1);
            //context.Bookings.Add(b2);
            //context.SaveChanges();

            //Edit
            //Pilot pilotToUpdate = pilotList.Where(p => p.FullName == "Meg Solliard ")
            //    .FirstOrDefault<Pilot>();
            //pilotToUpdate.FullName = "Meg Solliard Edited";
            //context.SaveChanges();
            //Console.Write("Update Done");


            //Delete
            //   context.Pilots.Remove(pilotList.ElementAt<Pilot>(0));
            //  context.SaveChanges();
            //   Console.Write("Remove Done");
        }
    }
}
