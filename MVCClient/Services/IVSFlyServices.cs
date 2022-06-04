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

        public Task<IEnumerable<PilotM>> GetPilots();
        public Task<IEnumerable<PassengerM>> GetPassengers();
        public Task<IEnumerable<BookingM>> GetBookings();

        public Task<FlightM> GetFlight(int id);

      

        public Task<PassengerM> GetPassengerByPassportID(string passportId);
        public  Task<PassengerM> GetPassenger(int Id); 
        public Task<PilotM> GetPilot(int Id);
        public Task<BookingM> GetBooking(int id);

        public Boolean CreatePassenger(PassengerM student);
        public Boolean CreateBooking(BookingM booking);

    }
}
