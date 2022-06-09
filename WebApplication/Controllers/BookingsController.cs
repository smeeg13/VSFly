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

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingM>>> GetBookings()
        {
            var BookingList = await _context.Bookings.ToListAsync();
            List<BookingM> bookingMList = new List<BookingM>();
            foreach(Booking b in BookingList)
            {
                var BM = b.ConvertToBookingM();
                bookingMList.Add(BM);
            }

            return bookingMList;
        }

        


        //Get Tickets by destination
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
        //GET Ticket for One Specific passenger
        [Route("Tickets/PassengerId/{passengerId:int}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketByPassengerId(int passengerId)
        {
            var booking = await _context.Bookings.Where(b => b.PassengerID == passengerId).Include(p => p.Passenger).Include(p => p.Flight).ToListAsync();
           // var passengers = await _context.Passengers.Where(p => p.PersonId == passengerId).ToListAsync();

            if (booking == null)
            {
                return NotFound();
            }
            List<Ticket> tickets = new ();
            //for (int i = 0; i < passengers.Count(); i++)
            //{
            //    for (int j = 0; j < booking.Count(); i++)
            //    {
            //        if (passengers.ElementAt(j).PersonId == booking.ElementAt(i).PassengerID)
            //        {
            //            booking.ElementAt(i).Passenger = passengers.ElementAt(j);
            //        }

            //    }
            //}
            foreach (Booking b in booking)
            {
                
                Ticket ticket = ConverterExtensions.GenerateTicket(b);
                tickets.Add(ticket);

            }
          

            return tickets;
        }


        //GET by flightNo
        [Route("FlightNo/{flightNo:int}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingM>>> GetBookingByFlightNo(int flightNo)
        {
            var booking = await _context.Bookings.Where(b => b.FlightNo == flightNo).ToListAsync();

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

        //GET by passengerId
        [Route("PassengerId/{passengerId:int}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingM>>> GetBookingByPassengerId(int passengerId)
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

        


        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingM>> GetBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            BookingM bookingM = booking.ConvertToBookingM();

            return bookingM;
        }

        // GET: api/Bookings/5
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

        // PUT: api/Bookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, BookingM bookingM)
        {
            if (id != bookingM.FlightNo)
            {
                return BadRequest();
            }

            _context.Entry(bookingM).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

            return CreatedAtAction("GetBooking", new { id = bookingM.FlightNo }, bookingM);
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.FlightNo == id);
        }
    }
}
