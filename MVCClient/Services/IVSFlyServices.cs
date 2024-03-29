﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVCClient.Models;

namespace MVCClient.Services
{
   public interface IVSFlyServices
    {
        public Task<IEnumerable<FlightM>> GetFlights();
        public Task<IEnumerable<FlightM>> GetAllFlights();

        public Task<IEnumerable<PilotAdminM>> GetPilots();
        
       // public Task<IEnumerable<BookingM>> GetBookings();

        public Task<FlightM> GetFlight(int id);
        public Task<IEnumerable<FlightAdminM>> GetFlightsByPilotId(int id);
        public  Task<IEnumerable<FlightAdminM>> GetFlightsByCoPilotId(int id);



        public Task<PassengerM> GetPassengerByPassportID(string passportId);
        public  Task<PassengerM> GetPassenger(int Id);
        public Task<PilotAdminM> GetPilotByPassportID(string passportId);
        public Task<PilotAdminM> GetPilot(int Id);
   
        public Task<BookingM> GetSpecificBooking(int FlightNo, int PersonId);
        public Task<Ticket> GetTicket(int flightNo, int personId);

        public Task<IEnumerable<BookingM>> GetBookingsByPassengerId(int id);
        public Task<IEnumerable<Ticket>> GetTicketsByPassengerId(int id);
        public Task<IEnumerable<Ticket>> GetTicketsByDestination(string dest);


        public Task<IEnumerable<Ticket>> GetTicketsByFlightNo(int id);
        public Task<IEnumerable<Destination>> GetAllDestinations();
        public Task<IEnumerable<FlightAdminM>> GetFlightsForDestination(string destinationName);



        public Boolean CreatePassenger(PassengerM student);
        public Boolean UpdatePassenger(PassengerM p);
       

public Boolean CreateBooking(BookingM booking);

        //_____________________ADMIN METHODS_________________________________
        public  Task<IEnumerable<FlightAdminM>> GetAllAdminFlights();
        public  Task<FlightAdminM> GetAdminFlight(int id);
        public Boolean CreateFlight(FlightAdminM f);
        public Boolean DeleteFlight(int id);
        public Boolean DeletePilot(int id);
        public Boolean DeletePassenger(int id);
 public Boolean UpdatePilot(PilotAdminM p);

        public Boolean UpdateFlight(FlightAdminM f);


        public Boolean CreatePilot(PilotAdminM f);



        public Task<IEnumerable<PassengerM>> GetPassengers();

    }
}
