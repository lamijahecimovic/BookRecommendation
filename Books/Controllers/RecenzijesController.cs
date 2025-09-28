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
    public class RecenzijesController : ControllerBase
    {
        private readonly KnjigeContext _context;

        public RecenzijesController(KnjigeContext context)
        {
            _context = context;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecenzije(int id, Recenzije recenzije)
        {
            if (id != recenzije.RecenzijaId)
            {
                return BadRequest();
            }

            _context.Entry(recenzije).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecenzijeExists(id))
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

        // POST: api/Recenzijes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
   

        // DELETE: api/Recenzijes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecenzije(int id)
        {
            var recenzije = await _context.Recenzijes.FindAsync(id);
            if (recenzije == null)
            {
                return NotFound();
            }

            _context.Recenzijes.Remove(recenzije);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecenzijeExists(int id)
        {
            return _context.Recenzijes.Any(e => e.RecenzijaId == id);
        }
        [HttpPost]
        public async Task<IActionResult> DodajRecenziju([FromBody] RecenzijaDto dto)
        {
            if (dto == null)
                return BadRequest("Podaci nisu validni");

            var recenzija = new Recenzije
            {
                KorisnikId = dto.KorisnikId,
                KnjigaId = dto.KnjigaId,
                Komentar = dto.Komentar,
                Ocjena = dto.Ocjena,
                DatumRecenzije = DateTime.Now
            };

            _context.Recenzijes.Add(recenzija);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("knjiga/{knjigaId}")]
        public async Task<IActionResult> GetByKnjiga(int knjigaId)
        {
            var recenzije = await _context.Recenzijes
                .Where(r => r.KnjigaId == knjigaId)
                .Include(r => r.Korisnik)
                .OrderByDescending(r => r.DatumRecenzije)
                .Select(r => new
                {
                    r.Komentar,
                    r.Ocjena,
                    r.DatumRecenzije,
                    korisnickoIme = r.Korisnik.KorisnickoIme
                })
                .ToListAsync();

            return Ok(recenzije);
        }
    }
}

