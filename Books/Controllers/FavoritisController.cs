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
    public class FavoritisController : ControllerBase
    {
        private readonly KnjigeContext _context;

        public FavoritisController(KnjigeContext context)
        {
            _context = context;
        }

        // GET: api/Favoritis
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Favoriti>>> GetFavoritis()
        {
            return await _context.Favoritis.ToListAsync();
        }

        // GET: api/Favoritis/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Favoriti>> GetFavoriti(int id)
        {
            var favoriti = await _context.Favoritis.FindAsync(id);

            if (favoriti == null)
            {
                return NotFound();
            }

            return favoriti;
        }



        [HttpPost("dodaj")]
        public async Task<IActionResult> DodajFavorita([FromBody] FavoritDto dto)
        {
            var postoji = await _context.Favoritis
                .AnyAsync(f => f.KorisnikId == dto.KorisnikId && f.KnjigaId == dto.KnjigaId);

            if (postoji)
                return BadRequest("Knjiga je već u favoritima.");

            var favorit = new Favoriti
            {
                KorisnikId = dto.KorisnikId,
                KnjigaId = dto.KnjigaId
            };

            _context.Favoritis.Add(favorit);
            await _context.SaveChangesAsync();

            return Ok();
        }
        [HttpGet("korisnik/{korisnikId}")]
        public async Task<IActionResult> GetFavoritis(int korisnikId)
        {
            var favoriti = await _context.Favoritis
                .Where(f => f.KorisnikId == korisnikId)
                .Include(f => f.Knjiga)
                .Select(f => new
                {
                    f.Knjiga!.KnjigaId,
                    f.Knjiga.Naslov,
                    f.Knjiga.Autor,
                    f.Knjiga.SlikaUrl
                })
                .ToListAsync();

            return Ok(favoriti);
        }
        // DELETE: api/Favoritis/5
        [HttpDelete("ukloni")]
        public async Task<IActionResult> UkloniFavorita(int korisnikId, int knjigaId)
        {
            var favorit = await _context.Favoritis
                .FirstOrDefaultAsync(f => f.KorisnikId == korisnikId && f.KnjigaId == knjigaId);

            if (favorit == null)
                return NotFound();

            _context.Favoritis.Remove(favorit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
