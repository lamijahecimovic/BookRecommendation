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
    public class KomentarisController : ControllerBase
    {
        private readonly KnjigeContext _context;

        public KomentarisController(KnjigeContext context)
        {
            _context = context;
        }

        // GET: api/Komentaris
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Komentari>>> GetKomentaris()
        {
            return await _context.Komentaris.ToListAsync();
        }

        // GET: api/Komentaris/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Komentari>> GetKomentari(int id)
        {
            var komentari = await _context.Komentaris.FindAsync(id);

            if (komentari == null)
            {
                return NotFound();
            }

            return komentari;
        }

        // PUT: api/Komentaris/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKomentari(int id, Komentari komentari)
        {
            if (id != komentari.KomentarId)
            {
                return BadRequest();
            }

            _context.Entry(komentari).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KomentariExists(id))
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

        // POST: api/Komentaris
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Komentari>> PostKomentari(Komentari komentari)
        {
            _context.Komentaris.Add(komentari);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (KomentariExists(komentari.KomentarId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetKomentari", new { id = komentari.KomentarId }, komentari);
        }

        // DELETE: api/Komentaris/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKomentari(int id)
        {
            var komentari = await _context.Komentaris.FindAsync(id);
            if (komentari == null)
            {
                return NotFound();
            }

            _context.Komentaris.Remove(komentari);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KomentariExists(int id)
        {
            return _context.Komentaris.Any(e => e.KomentarId == id);
        }
    }
}
