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

        // GET: api/Pilots
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PilotAdminM>>> GetPilots()
        {
            var PilotList = await _context.Pilots.ToListAsync();
            List<PilotAdminM> pilotMList = new List<PilotAdminM>();
            foreach (Pilot p in PilotList)
            {
                var PM = p.ConvertToPilotAdminM();
                pilotMList.Add(PM);
            }
            return pilotMList;
        }

        // GET: api/Pilots/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PilotM>> GetPilot(int id)
        {
            var pilot = await _context.Pilots.FindAsync(id);

            if (pilot == null)
            {
                return NotFound();
            }

            PilotM pilotM = pilot.ConvertToPilotM();

            return pilotM;
        }

       

        private bool PilotExists(int id)
        {
            return _context.Pilots.Any(e => e.PersonId == id);
        }



        //_____________________ADMIN METHODS_________________________________
        
        // PUT: api/Pilots/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Admin/UpdatePilot/{id:int}")]
        public async Task<IActionResult> PutPilot(int id, PilotAdminM pilotM)
        {
            if (id != pilotM.PersonId)
            {
                return BadRequest();
            }

            _context.Entry(pilotM).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PilotExists(id))
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

        // POST: api/Pilots
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        // DELETE: api/Pilots/5
        [HttpDelete("Admin/DeletePilot/{id}")]
        public async Task<IActionResult> DeletePilot(int id)
        {
            var pilot = await _context.Pilots.FindAsync(id);
            if (pilot == null)
            {
                return NotFound();
            }

            _context.Pilots.Remove(pilot);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
