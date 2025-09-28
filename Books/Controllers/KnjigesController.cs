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
    public class KnjigesController : ControllerBase
    {
        private readonly KnjigeContext _context;

        public KnjigesController(KnjigeContext context)
        {
            _context = context;
        }

        // GET: api/Knjiges
        [HttpGet]
        public async Task<IActionResult> GetSveKnjige()
        {
            var knjige = await _context.Knjiges
                .Select(k => new {
                    k.KnjigaId,
                    k.Naslov,
                    k.Autor,
                    k.Opis,
                    k.SlikaUrl,
                    k.PdfUrl
                }).ToListAsync();

            return Ok(knjige);
        }



        // GET: api/Knjiges/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Knjige>> GetKnjige(int id)
        {
            var knjige = await _context.Knjiges.FindAsync(id);

            if (knjige == null)
            {
                return NotFound();
            }

            return knjige;
        }

        // PUT: api/Knjiges/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKnjige(int id, Knjige knjige)
        {
            if (id != knjige.KnjigaId)
            {
                return BadRequest();
            }

            _context.Entry(knjige).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KnjigeExists(id))
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

        // POST: api/Knjiges
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Knjige>> PostKnjige(Knjige knjige)
        {
            _context.Knjiges.Add(knjige);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (KnjigeExists(knjige.KnjigaId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetKnjige", new { id = knjige.KnjigaId }, knjige);
        }

        // DELETE: api/Knjiges/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKnjige(int id)
        {
            var knjige = await _context.Knjiges.FindAsync(id);
            if (knjige == null)
            {
                return NotFound();
            }

            _context.Knjiges.Remove(knjige);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KnjigeExists(int id)
        {
            return _context.Knjiges.Any(e => e.KnjigaId == id);
        }
  

    }

}
