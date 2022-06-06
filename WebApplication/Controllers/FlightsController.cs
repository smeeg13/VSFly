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
        [HttpGet]
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
        [Route("PilotId/{pilotId:int}")]
        [HttpGet]
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


        private bool FlightExists(int id)
        {
            return _context.Flights.Any(e => e.FlightNo == id);
        }

        //_____________________ADMIN METHODS_________________________________
        
        //Display all flights 
        // GET: api/Flights/Admin/All
        [Route("Admin/All")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlightAdminM>>> GetAdminFlights()
        {

            var flightList = await _context.Flights.ToListAsync();
            List<FlightAdminM> flightMList = new List<FlightAdminM>();
            foreach (Flight f in flightList)
            {

                var FM = f.ConvertToFlightAdminM();
                FM.SalePrice = getSalePrice(f.Seat, f.FreeSeats, f.Date, f.Price);

                flightMList.Add(FM);

            }

            flightList.OrderBy(f => f.FreeSeats);
            return flightMList;
        }
        //// GET: api/FlightsAdmin/5
        [HttpGet("Admin/{id}")]
        public async Task<ActionResult<FlightAdminM>> GetAdminFlight(int id)
        {
            var flight = await _context.Flights.FindAsync(id);

            if (flight == null)
            {
                return NotFound();
            }

            FlightAdminM flightM = flight.ConvertToFlightAdminM();
            flightM.SalePrice = getSalePrice(flight.Seat, flight.FreeSeats, flight.Date, flight.Price);

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
        [HttpPut("Admin/UpdateFlight/{id:int}")]
        public async Task<IActionResult> PutFlight(int id, FlightAdminM flightM)
        {
            if (id != flightM.FlightNo)
            {
                return BadRequest();
            }

            _context.Entry(flightM).State = EntityState.Modified;

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

            return NoContent();
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
