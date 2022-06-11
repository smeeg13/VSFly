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
    public class PassengersController : ControllerBase
    {
        private readonly VsflyContext _context;

        public PassengersController(VsflyContext context)
        {
            _context = context;
        }

        // GET ALL PASSENGERS IN DB
        // api/Passengers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PassengerM>>> GetPassengers()
        {
            var PassengerList = await _context.Passengers.ToListAsync();
            List<PassengerM> passengerMList = new List<PassengerM>();
            foreach(Passenger p in PassengerList)
            {
                var PM = p.ConvertToPassengerM();
            
                passengerMList.Add(PM);
            }

            return passengerMList;
        }

        // GET ONE PASSENGER BY ID
        // api/Passengers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PassengerM>> GetPassenger(int id)
        {
            var passenger = await _context.Passengers.FindAsync(id);

            if (passenger == null)
            {
                return null;
            }

            PassengerM passengerM = passenger.ConvertToPassengerM();


            return passengerM;
        }

        // GET ONE PASSENGER BY PASSPORT ID
        // api/Passengers/Find/2
        [HttpGet("Find/{passportId}")]
        public async Task<ActionResult<PassengerM>> GetPassengerByPassportID(string passportId)
        {
            var passengers = await _context.Passengers.ToListAsync() ;

            if (passengers == null)
            {
                return null;
            }

            PassengerM passengerM = new();
            foreach (Passenger p in passengers)
            {
                if (passportId.Equals(p.PassportID))
                {
                    passengerM = p.ConvertToPassengerM();
                }
            }

            return passengerM;
        }

        // PUT MODIFICATION OF A PASSENGER INTO DB
        // api/Passengers/UpdatePassenger/5
        [HttpPut("UpdatePassenger/{id}")]
        public async Task<IActionResult> PutPassenger(int id, PassengerM passengerM)
        {
            if (id != passengerM.PersonId)
            {
                return BadRequest();
            }
            var existingPassenger = _context.Passengers.Where(s => s.PersonId == passengerM.PersonId).FirstOrDefault<Passenger>();

            if (existingPassenger != null)
            {
                existingPassenger.FullName = passengerM.FullName;
                existingPassenger.PassportID = passengerM.PassportID;
                existingPassenger.Email = passengerM.Email;
                existingPassenger.Birthday = passengerM.Birthday;
                existingPassenger.Status = passengerM.Status;
            }
          
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PassengerExists(passengerM.PersonId))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST A NEW PASSENGER INTO DB
        // api/Passengers/CreatePassenger
        [Route("CreatePassenger")]
        [HttpPost]
        public async Task<ActionResult<PassengerM>> PostPassenger(PassengerM passengerM)
        {
            passengerM.Status = "Passenger";
            Passenger passenger = passengerM.ConvertToPassenger();
            passenger.CustomerSince = DateTime.Now;
            _context.Passengers.Add(passenger);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PassengerExists(passenger.PersonId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPassenger", new { id = passengerM.PersonId }, passengerM);
        }

        // DELETE A GIVEN PASSENGER
        // api/Passengers/Admin/DeletePassenger/5
        [HttpDelete("Admin/DeletePassenger/{id:int}")]
        public async Task<IActionResult> DeletePassenger(int id)
        {
            if (id <= 0)
                return NotFound();

            var Passenger = await _context.Passengers.FindAsync(id);
            if (Passenger == null)
            {
                return NotFound();
            }

            _context.Passengers.Remove(Passenger);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool PassengerExists(int id)
        {
            return _context.Passengers.Any(e => e.PersonId == id);
        }
    }
}
