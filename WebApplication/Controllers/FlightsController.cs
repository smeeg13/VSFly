using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Extensions;
using VSFly;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly VsflyContext _context;

        public FlightsController(VsflyContext context)
        {
            _context = context;
        }



        //Display all flights still available
        // GET: api/Flights
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlightM>>> GetFlights()
        {

            var flightList = await _context.Flights.ToListAsync();
            List<FlightM> flightMList = new List<FlightM>();
            foreach (Flight f in flightList)
            {
                //Check if flight have free seats
                if (f.FreeSeats != 0)
                {
                    var FM = f.ConvertToFlightM();
                    FM.SalePrice = getSalePrice(f.Seat, f.FreeSeats, f.Date, f.Price);

                    flightMList.Add(FM);
                }
            }
            return flightMList;
        }
        //Display all flights 
        // GET: api/Flights/All
        [Route("All")]
        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<FlightM>>> GetAllFlights()
        {

            var flightList = await _context.Flights.ToListAsync();
            List<FlightM> flightMList = new List<FlightM>();
            foreach (Flight f in flightList)
            {

                var FM = f.ConvertToFlightM();
                FM.SalePrice = getSalePrice(f.Seat, f.FreeSeats, f.Date, f.Price);

                flightMList.Add(FM);

            }

            flightList.OrderBy(f => f.FreeSeats);
            return flightMList;
        }
        //to get the sale price
        private double getSalePrice(int seat, int freeSeat, DateTime date, double price)
        {
            var priceSale = 0.0;
            //% of airplane full
            var ocup = seat - freeSeat;
            var full = ocup * 100 / seat;

            //see time before departure
            var year = date.Date.Year - DateTime.Now.Date.Year;
            var month = year * 12;
            month = +date.Date.Month - DateTime.Now.Date.Month;
            var days = month * 30;
            days += date.Date.Day - DateTime.Now.Date.Day;

            if (full > 80)
                priceSale = price * 150 / 100;

            else if (full < 20 && days < 60)
                priceSale = price * 80 / 100;

            else if (full < 50 && days < 30)
                priceSale = price * 70 / 100;

            else
                priceSale = price;

            return priceSale;
        }

        //GET sum all salePrice of a flight
        private double GetSumSalePrice(List<Booking> bookings)
        {
            //var bookingss = await _context.Bookings.Where(b => b.FlightNo == flightNo).ToListAsync();
            var sum = 0.0;
            //if (bookings == null)
            //{
            //    return NotFound();
            //}

            foreach (Booking b in bookings)
            {
                sum += b.SalePrice;
            }

            return sum;
        }


        //GET avg all salePrice of a or many flights
        private double GetAvgSalePrice(List<Flight> flights)
        {
            

            var nb = 0;
            var sum = 0.0;
            var avg = 0.0;
            foreach (Flight f in flights)
            {
                var bookings = _context.Bookings.Where(b => b.FlightNo == f.FlightNo).ToList();
                if (bookings != null)
                {
                    foreach (Booking b in bookings)
                    {
                        sum += b.SalePrice;
                        nb += 1;
                    }
                }
                else
                {
                    avg = 0.0;
                }
            }
            avg = sum / nb;

            return avg;
        }


        //// GET: api/Flights/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FlightM>> GetFlight(int id)
        {
            var flight = await _context.Flights.FindAsync(id);

            if (flight == null)
            {
                return NotFound();
            }

            FlightM flightM = flight.ConvertToFlightM();
            flightM.SalePrice = getSalePrice(flight.Seat, flight.FreeSeats, flight.Date, flight.Price);
            
            return flightM;
        }

        // GET: api/Flights/PilotId/5
        [HttpGet("PilotId/{pilotId:int}")]
        public async Task<ActionResult<IEnumerable<FlightAdminM>>> GetFlightsByPilotId(int pilotId)
        {
            var flights = await _context.Flights.Where(b => b.PilotId == pilotId).ToListAsync();

            if (flights == null)
            {
                return NotFound();
            }
            List<FlightAdminM> flightsMs = new();

            foreach (Flight f in flights)
            {

                FlightAdminM flightgM = f.ConvertToFlightAdminM();
                flightsMs.Add(flightgM);

            }

            return flightsMs;
        }

        // GET: api/Flights/PilotId/5
        [HttpGet("CoPilotId/{copilotId:int}")]
        public async Task<ActionResult<IEnumerable<FlightAdminM>>> GetFlightsByCoPilotId(int copilotId)
        {
            var flights = await _context.Flights.Where(b => b.CopilotId == copilotId).ToListAsync();

            if (flights == null)
            {
                return NotFound();
            }
            List<FlightAdminM> flightsMs = new();

            foreach (Flight f in flights)
            {

                FlightAdminM flightgM = f.ConvertToFlightAdminM();
                flightsMs.Add(flightgM);

            }

            return flightsMs;
        }

        //Get all destination Available
        // GET: api/Flights/Destinations
        [Route("Destinations")]
        [HttpGet("Destinations")]
        public async Task<ActionResult<IEnumerable<Destination>>> GetAllDestinations()
        {

            var flightsAvailable = await _context.Flights.Where(x=>x.FreeSeats > 0).ToListAsync();
            var destNames = flightsAvailable.Select(x => x.Destination).Distinct().ToList();
            
            List<Destination> destinations = new List<Destination>();
            for(int i=0; i<destNames.Count(); i++)
            {
                var newDest = new Destination(destNames.ElementAt(i).ToString());
                destinations.Add(newDest);
            }
            if (destinations != null)
            {
                foreach (Destination dest in destinations)
                {
                    var flightForDest = flightsAvailable.Where(x => x.Destination == dest.DestinationName).ToList();
                    List<FlightAdminM> flightAdminForDest = new();
                    foreach (Flight f in flightForDest)
                    {

                        var FM = f.ConvertToFlightAdminM();
                        FM.SalePrice = getSalePrice(f.Seat, f.FreeSeats, f.Date, f.Price);

                        flightAdminForDest.Add(FM);

                    }
                    dest.Flights = flightAdminForDest;

                }
            }

            foreach (Destination dest2 in destinations)
            {

                foreach (FlightAdminM f in dest2.Flights)
                {
                    var bookings = await _context.Bookings.Where(b => b.Flight.Destination == dest2.DestinationName).ToListAsync();
                    if (bookings != null)
                    {
                        dest2.SumSales = GetSumSalePrice(bookings);
                        //var avgflights = await _context.Flights.Where(f => f.Destination == dest2.DestinationName).ToListAsync();
                        //dest2.AvgSales = GetAvgSalePrice(avgflights);
                    }
                    else
                    {
                        dest2.SumSales = 0;
                    }
                }
            }


            return destinations;
        }

        private bool FlightExists(int id)
        {
            return _context.Flights.Any(e => e.FlightNo == id);
        }

        //_____________________ADMIN METHODS_________________________________
        
        //Display all flights 
        // GET: api/Flights/Admin/All
        [Route("Admin/All")]
       // [HttpGet("Admin/All")]
        public async Task<ActionResult<IEnumerable<FlightAdminM>>> GetAdminFlights()
        {

            var flightList = await _context.Flights.ToListAsync();
            List<FlightAdminM> flightMList = new List<FlightAdminM>();
            foreach (Flight f in flightList)
            {

                var FM = f.ConvertToFlightAdminM();
                FM.SalePrice = getSalePrice(f.Seat, f.FreeSeats, f.Date, f.Price);
                var bookings = await _context.Bookings.Where(b => b.FlightNo == FM.FlightNo).ToListAsync();
                if (bookings != null)
                {
                    FM.SumSales = GetSumSalePrice(bookings);

                }
                else
                {
                    FM.SumSales = 0;
                }
                flightMList.Add(FM);

            }

            flightList.OrderBy(f => f.FreeSeats);
            return flightMList;
        }
        //// GET: api/Flights/Admin/5
        [HttpGet("Admin/FlightNo/{id}")]
        public async Task<ActionResult<FlightAdminM>> GetAdminFlight(int id)
        {
            var flight = await _context.Flights.FindAsync(id);

            if (flight == null)
            {
                return NotFound();
            }

            FlightAdminM flightM = flight.ConvertToFlightAdminM();
            flightM.SalePrice = getSalePrice(flight.Seat, flight.FreeSeats, flight.Date, flight.Price);
            var bookings = await _context.Bookings.Where(b => b.FlightNo == flightM.FlightNo).ToListAsync();
            if (bookings != null)
            {
            flightM.SumSales = GetSumSalePrice(bookings);

            }
            else
            {
                flightM.SumSales = 0;
            }

            return flightM;
        }

        // GET: api/Flights/Admin/PilotId/5
        [HttpGet("Admin/PilotId/{pilotId:int}")]
        public async Task<ActionResult<IEnumerable<FlightAdminM>>> GetAdminFlightsByPilotId(int pilotId)
        {
            var flights = await _context.Flights.Where(b => b.PilotId == pilotId).ToListAsync();

            if (flights == null)
            {
                return NotFound();
            }
            List<FlightAdminM> flightsMs = new();

            foreach (Flight f in flights)
            {

                FlightAdminM flightgM = f.ConvertToFlightAdminM();
                flightsMs.Add(flightgM);

            }

            return flightsMs;
        }

        // POST: api/Flights/Admin/CreateFlight
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("Admin/CreateFlight")]
        [HttpPost]
        public async Task<ActionResult<FlightAdminM>> PostFlight(FlightAdminM flightM)
        {
            flightM.FreeSeats = flightM.Seat;

            _context.Flights.Add(flightM.ConvertToFlightFromAdmin());
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFlight", new { id = flightM.FlightNo }, flightM);
        }
        // PUT: api/Flights/Admin/UpdateFlight/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> PutFlight(int id, FlightAdminM flightM)
        {
            if (id != flightM.FlightNo)
            {
                return BadRequest();
            }
            var existingFlight = _context.Flights.Where(s => s.FlightNo == flightM.FlightNo).FirstOrDefault<Flight>();

            if (existingFlight != null)
            {
                existingFlight.AirlineName = flightM.AirlineName;
                existingFlight.Destination = flightM.Destination;
                existingFlight.Departure = flightM.Departure;
                existingFlight.CopilotId = flightM.CopilotId;
                existingFlight.PilotId = flightM.PilotId;
                existingFlight.Price = flightM.Price;
                existingFlight.Seat = flightM.Seat;

            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlightExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // DELETE: api/Flights/5
        [HttpDelete("Admin/DeleteFlight/{id:int}")]
        public async Task<IActionResult> DeleteFlight(int id)
        {
            var flight = await _context.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound();
            }

            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
