using System;
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
        
        public Task<IEnumerable<BookingM>> GetBookings();

        public Task<FlightM> GetFlight(int id);
        public Task<IEnumerable<FlightAdminM>> GetFlightsByPilotId(int id);



        public Task<PassengerM> GetPassengerByPassportID(string passportId);
        public  Task<PassengerM> GetPassenger(int Id); 
        public Task<PilotAdminM> GetPilot(int Id);
   
        public Task<BookingM> GetSpecificBooking(int FlightNo, int PersonId);
        public Task<Ticket> GetTicket(int flightNo, int personId);

        public Task<IEnumerable<BookingM>> GetBookingByPassengerId(int id);
        public  Task<IEnumerable<Ticket>> GetTicketsByPassengerId(int id);

        public Task<IEnumerable<BookingM>> GetBookingByFlightNo(int id);


        public Boolean CreatePassenger(PassengerM student);
        public Boolean CreateBooking(BookingM booking);

        //_____________________ADMIN METHODS_________________________________
        public  Task<IEnumerable<FlightAdminM>> GetAllAdminFlights();
        public  Task<FlightAdminM> GetAdminFlight(int id);
        public Boolean CreateFlight(FlightAdminM f);
        public Boolean CreatePilot(PilotAdminM f);



        public Task<IEnumerable<PassengerM>> GetPassengers();

    }
}
