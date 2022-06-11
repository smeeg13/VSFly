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
    public class PilotsController : ControllerBase
    {
        private readonly VsflyContext _context;

        public PilotsController(VsflyContext context)
        {
            _context = context;
        }

        // GET ALL PILOTS (admin)
        // api/Pilots
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PilotAdminM>>> GetPilots()
        {
            var PilotList = await _context.Pilots.ToListAsync();
            List<PilotAdminM> pilotMList = new List<PilotAdminM>();
            foreach (Pilot p in PilotList)
            {
                var PM = p.ConvertToPilotAdminM();
                if(PM.Salary<=0 || PM.Salary == null)
                {
                    PM.Salary = 40000;
                }
                pilotMList.Add(PM);
            }
            return pilotMList;
        }

        // GET ONE PILOT BY ID (admin)
        // api/Pilots/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PilotAdminM>> GetPilot(int id)
        {
            var pilot = await _context.Pilots.FindAsync(id);

            if (pilot == null)
            {
                return null;
            }

            PilotAdminM pilotM = pilot.ConvertToPilotAdminM();
            if (pilotM.Salary <= 0 || pilotM.Salary == null)
            {
                pilotM.Salary = 40000;
            }
            return pilotM;
        }
        // GET ONE PILOT BY PASSPORT ID
        // api/Pilots/Find/2
        [HttpGet("Find/{passportId}")]
        public async Task<ActionResult<PilotAdminM>> GetPilotByPassportID(string passportId)
        {
            var pilots = await _context.Pilots.ToListAsync();

            if (pilots == null)
            {
                return null;
            }

            PilotAdminM pilotM = null;
            foreach (Pilot p in pilots)
            {
                if (passportId.Equals(p.PassportID))
                {
                    pilotM = p.ConvertToPilotAdminM();
                    if (pilotM.Salary <= 0 || pilotM.Salary == null)
                    {
                        pilotM.Salary = 40000;
                    }
                }
            }
            if (pilotM != null)
                return pilotM;
            else
                return null;
        }

        //CHECK IF THE PILOT EXISTS WITH HIS ID
        private bool PilotExists(int id)
        {
            return _context.Pilots.Any(e => e.PersonId == id);
        }



        //_____________________ADMIN METHODS_________________________________

        // PUT PILOT MODIFICATION INTO BD
        // api/Pilots/Update/5
        [HttpPut("Admin/UpdatePilot/{id}")]
        public async Task<IActionResult> PutPilot(int id, PilotAdminM pilotM)
        {
            if (id != pilotM.PersonId)
            {
                return BadRequest();
            }
            var existingPassenger = _context.Pilots.Where(s => s.PersonId == pilotM.PersonId).FirstOrDefault<Pilot>();

            if (existingPassenger != null)
            {
                existingPassenger.FullName = pilotM.FullName;
                existingPassenger.PassportID = pilotM.PassportID;
                existingPassenger.Email = pilotM.Email;
                existingPassenger.Birthday = pilotM.Birthday;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PilotExists(pilotM.PersonId))
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
        // POST NEW PILOT INTO DB
        // api/Pilots/Admin/CreatePilot
        [Route("Admin/CreatePilot")]
        [HttpPost]
        public async Task<ActionResult<PilotAdminM>> PostPilot(PilotAdminM pilotM)
        {
            Pilot pilot = pilotM.ConvertToPilotFromAdmin();
            _context.Pilots.Add(pilot);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PilotExists(pilot.PersonId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
                return CreatedAtAction("GetPilot", new { id = pilotM.PersonId }, pilotM);
        }

        // DELETE ON PILOT OF THE BD
        // api/Pilots/Admin/DeletePilot/5
        [HttpDelete("Admin/DeletePilot/{id:int}")]
        public async Task<IActionResult> DeletePilot(int id)
        {
            if (id <= 0)
                return NotFound();

            var pilot = await _context.Pilots.FindAsync(id);
            if (pilot == null)
            {
                return NotFound();
            }

            _context.Pilots.Remove(pilot);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
