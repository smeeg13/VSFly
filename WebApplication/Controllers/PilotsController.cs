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
        public async Task<ActionResult<IEnumerable<PilotM>>> GetPilots()
        {
            var PilotList = await _context.Pilots.ToListAsync();
            List<PilotM> pilotMList = new List<PilotM>();
            foreach (Pilot p in PilotList)
            {
                var PM = p.ConvertToPilotM();
                pilotMList.Add(PM);
            }
            return pilotMList;
        }

        // GET: api/Pilots/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pilot>> GetPilot(int id)
        {
            var pilot = await _context.Pilots.FindAsync(id);

            if (pilot == null)
            {
                return NotFound();
            }

            return pilot;
        }

        // PUT: api/Pilots/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPilot(int id, Pilot pilot)
        {
            if (id != pilot.PersonId)
            {
                return BadRequest();
            }

            _context.Entry(pilot).State = EntityState.Modified;

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
        [HttpPost]
        public async Task<ActionResult<Pilot>> PostPilot(Pilot pilot)
        {
            _context.Pilots.Add(pilot);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPilot", new { id = pilot.PersonId }, pilot);
        }

        // DELETE: api/Pilots/5
        [HttpDelete("{id}")]
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

        private bool PilotExists(int id)
        {
            return _context.Pilots.Any(e => e.PersonId == id);
        }
    }
}
