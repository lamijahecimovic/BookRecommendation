using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Books.Models;

namespace Books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreporukasController : ControllerBase
    {
        private readonly KnjigeContext _context;

        public PreporukasController(KnjigeContext context)
        {
            _context = context;
        }

        // GET: api/Preporukas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Preporuka>>> GetPreporukas()
        {
            return await _context.Preporukas.ToListAsync();
        }

        // GET: api/Preporukas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Preporuka>> GetPreporuka(int id)
        {
            var preporuka = await _context.Preporukas.FindAsync(id);

            if (preporuka == null)
            {
                return NotFound();
            }

            return preporuka;
        }

        // PUT: api/Preporukas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPreporuka(int id, Preporuka preporuka)
        {
            if (id != preporuka.PreporukaId)
            {
                return BadRequest();
            }

            _context.Entry(preporuka).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PreporukaExists(id))
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

        // POST: api/Preporukas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Preporuka>> PostPreporuka(Preporuka preporuka)
        {
            _context.Preporukas.Add(preporuka);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PreporukaExists(preporuka.PreporukaId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPreporuka", new { id = preporuka.PreporukaId }, preporuka);
        }

        // DELETE: api/Preporukas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePreporuka(int id)
        {
            var preporuka = await _context.Preporukas.FindAsync(id);
            if (preporuka == null)
            {
                return NotFound();
            }

            _context.Preporukas.Remove(preporuka);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PreporukaExists(int id)
        {
            return _context.Preporukas.Any(e => e.PreporukaId == id);
        }
    }
}
