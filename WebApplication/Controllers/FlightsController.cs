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

           var flightList=  await _context.Flights.ToListAsync();
            List<FlightM> flightMList = new List<FlightM>();
            foreach (Flight f in flightList)
            {
                //Check if flight have free seats
                if (f.FreeSeats!= 0) {
                    var FM = f.ConvertToFlightM();

                    FM.SalePrice = getSalePrice(f.Seat, f.FreeSeats, f.Date, f.Price);

                    flightMList.Add(FM);
                }
            }
            return flightMList;
        }
        //to get the sale price
        public double getSalePrice(int seat, int freeSeat, DateTime date, double price)
        {
            var priceSale = 0.0;
            //% of airplane full
            var ocup = seat - freeSeat;
            var full = ocup * 100 / seat;

            //see time before departure
            var year = date.Date.Year - DateTime.Now.Date.Year;
            var month = year * 12;
            month = + date.Date.Month - DateTime.Now.Date.Month;
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


        // GET: api/Flights/5
        [Route("api/Flights/{id:int}")]
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

        // PUT: api/Flights/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFlight(int id, FlightM flightM)
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

        // POST: api/Flights
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FlightM>> PostFlight(FlightM flightM)
        {

            _context.Flights.Add(flightM.ConvertToFlight());
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFlight", new { id = flightM.FlightNo }, flightM);
        }

        // DELETE: api/Flights/5
        [HttpDelete("{id}")]
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

        private bool FlightExists(int id)
        {
            return _context.Flights.Any(e => e.FlightNo == id);
        }
    }
}
