using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VSFly;
using WebAPI.Extensions;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly VsflyContext _context;

        public BookingsController(VsflyContext context)
        {
            _context = context;
        }


        //Get ALL Tickets FOR ONE destination
        [Route("Tickets/Destination/{destination}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketsByDestination(string destination)
        {
            List<Ticket> tickets = new List<Ticket>();
            var flights = await _context.Flights.Where(f => f.Destination == destination).ToListAsync();
            if (flights == null)
            {
                return NotFound();
            }

            foreach (Flight f in flights)
            {
                var bookings = await _context.Bookings.Where(b => b.FlightNo == f.FlightNo).Include(x => x.Passenger).ToListAsync();

                foreach (Booking b in bookings)
                {
                    Ticket ticket = new Ticket { FullName = b.Passenger.FullName, FlightNo = f.FlightNo, SalePrice = b.SalePrice, Date = f.Date, Destination = f.Destination,Departure = f.Departure, PassengerId = b.PassengerID };
                    tickets.Add(ticket);
                }
            }
            return tickets;
        }

        //Get Ticket for One Specific Booking
        [Route("Tickets/{flightNo}/{personId}")]
        [HttpGet]
        public async Task<ActionResult<List<Ticket>>> GetTicket(int flightNo, int personId)
        {
            var bookings = await _context.Bookings.Where(b => b.FlightNo == flightNo).Where(b1 => b1.PassengerID ==personId).Include(x => x.Flight).Include(y => y.Passenger).Distinct().ToListAsync();
            if (bookings == null)
            {
                return NotFound();
            }
            List<Ticket> tickets = new();

            foreach (Booking f in bookings)
            {
                Ticket ticket = ConverterExtensions.GenerateTicket(f);
                tickets.Add(ticket);
            }
            return tickets;
        }
        //GET All Tickets for One Specific passenger
        [Route("Tickets/PassengerId/{passengerId:int}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketByPassengerId(int passengerId)
        {
            var booking = await _context.Bookings.Where(b => b.PassengerID == passengerId).Include(p => p.Passenger).Include(p => p.Flight).ToListAsync();

            if (booking == null)
            {
                return NotFound();
            }
            List<Ticket> tickets = new ();
           
            foreach (Booking b in booking)
            {
                
                Ticket ticket = ConverterExtensions.GenerateTicket(b);
                tickets.Add(ticket);

            }
          

            return tickets;
        }


        //GET by flightNo
        [Route("TicketsForFlight/{flightNo:int}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketsByFlightNo(int flightNo)
        {
            var booking = await _context.Bookings.Where(b => b.FlightNo == flightNo).Include(p=>p.Passenger).Include(f=>f.Flight).ToListAsync();

            if (booking == null)
            {
                return NotFound();
            }
            List<Ticket> tickets = new List<Ticket>();

            foreach (Booking b in booking)
            {

                Ticket bookingM = b.GenerateTicket();
                tickets.Add(bookingM);

            }

            return tickets;
        }

        //GET bookings by passengerId
        [Route("PassengerId/{passengerId:int}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingM>>> GetBookingsByPassengerId(int passengerId)
        {
            var booking = await _context.Bookings.Where(b => b.PassengerID == passengerId).ToListAsync();

            if (booking == null)
            {
                return NotFound();
            }
            List<BookingM> bookingMs = new List<BookingM>();

            foreach (Booking b in booking)
            {

                BookingM bookingM = b.ConvertToBookingM();
                bookingMs.Add(bookingM);

            }

            return bookingMs;
        }

        

        // GET Booking by FlightNo & PassengerID
        // api/Bookings/5
        [HttpGet("Find/{FlightNo}/{PersonId}")]
        public async Task<ActionResult<BookingM>> GetSpecificBooking(int FlightNo, int PersonId)
        {
            var bookings = await _context.Bookings.Where(e => e.FlightNo == FlightNo).ToListAsync();
            var booking = new Booking();
           

            if (bookings == null)
            {
                return NotFound();
            }

            foreach (Booking b in bookings)
            {
                if (b.PassengerID == PersonId)
                {
                    booking = b;
                }
            }
            BookingM bookingM = booking.ConvertToBookingM();

            return bookingM;
        }

      

        // POST A NEW BOOKING
        // api/Bookings
        [Route("Create")]
        [HttpPost]
        public async Task<ActionResult<BookingM>> PostBooking(BookingM bookingM)
        {
            Booking booking = bookingM.ConvertToBooking();
            _context.Bookings.Add(booking);
            //-1 Free Seat for the corresponding flight
            var flight = _context.Flights.Where(x=>x.FlightNo == booking.FlightNo).FirstOrDefault();
            flight.FreeSeats = flight.FreeSeats - 1;
            _context.Entry(flight).State = EntityState.Modified;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BookingExists(booking.FlightNo))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

      

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.FlightNo == id);
        }
    }
}
