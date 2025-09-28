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
    public class HistorijasController : ControllerBase
    {
        private readonly KnjigeContext _context;

        public HistorijasController(KnjigeContext context)
        {
            _context = context;
        }

        // GET: api/Historijas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Historija>>> GetHistorijas()
        {
            return await _context.Historijas.ToListAsync();
        }

        // GET: api/Historijas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Historija>> GetHistorija(int id)
        {
            var historija = await _context.Historijas.FindAsync(id);

            if (historija == null)
            {
                return NotFound();
            }

            return historija;
        }

        // PUT: api/Historijas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHistorija(int id, Historija historija)
        {
            if (id != historija.HistorijaId)
            {
                return BadRequest();
            }

            _context.Entry(historija).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HistorijaExists(id))
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

        // POST: api/Historijas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Historija>> PostHistorija(Historija historija)
        {
            _context.Historijas.Add(historija);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (HistorijaExists(historija.HistorijaId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetHistorija", new { id = historija.HistorijaId }, historija);
        }

        // DELETE: api/Historijas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHistorija(int id)
        {
            var historija = await _context.Historijas.FindAsync(id);
            if (historija == null)
            {
                return NotFound();
            }

            _context.Historijas.Remove(historija);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HistorijaExists(int id)
        {
            return _context.Historijas.Any(e => e.HistorijaId == id);
        }
    }
}
