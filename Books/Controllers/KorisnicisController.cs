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
    public class KorisnicisController : ControllerBase
    {
        private readonly KnjigeContext _context;

        public KorisnicisController(KnjigeContext context)
        {
            _context = context;
        }

        // GET: api/Korisnicis
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Korisnici>>> GetKorisnicis()
        {
            return await _context.Korisnicis.ToListAsync();
        }

        // GET: api/Korisnicis/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Korisnici>> GetKorisnici(int id)
        {
            var korisnici = await _context.Korisnicis.FindAsync(id);

            if (korisnici == null)
            {
                return NotFound();
            }

            return korisnici;
        }

        // PUT: api/Korisnicis/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKorisnici(int id, Korisnici korisnici)
        {
            if (id != korisnici.KorisnikId)
            {
                return BadRequest();
            }

            _context.Entry(korisnici).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KorisniciExists(id))
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

        // POST: api/Korisnicis
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Korisnici>> PostKorisnici(Korisnici korisnici)
        {
            _context.Korisnicis.Add(korisnici);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (KorisniciExists(korisnici.KorisnikId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetKorisnici", new { id = korisnici.KorisnikId }, korisnici);
        }

        // DELETE: api/Korisnicis/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKorisnici(int id)
        {
            var korisnici = await _context.Korisnicis.FindAsync(id);
            if (korisnici == null)
            {
                return NotFound();
            }

            _context.Korisnicis.Remove(korisnici);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KorisniciExists(int id)
        {
            return _context.Korisnicis.Any(e => e.KorisnikId == id);
        }
    }
}
