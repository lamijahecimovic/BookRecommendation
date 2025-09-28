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
    public class ZanrsController : ControllerBase
    {
        private readonly KnjigeContext _context;

        public ZanrsController(KnjigeContext context)
        {
            _context = context;
        }

        // GET: api/Zanrs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Zanr>>> GetZanrs()
        {
            return await _context.Zanrs.ToListAsync();
        }

        // GET: api/Zanrs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Zanr>> GetZanr(int id)
        {
            var zanr = await _context.Zanrs.FindAsync(id);

            if (zanr == null)
            {
                return NotFound();
            }

            return zanr;
        }

        // PUT: api/Zanrs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutZanr(int id, Zanr zanr)
        {
            if (id != zanr.ZanrId)
            {
                return BadRequest();
            }

            _context.Entry(zanr).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ZanrExists(id))
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

        // POST: api/Zanrs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Zanr>> PostZanr(Zanr zanr)
        {
            _context.Zanrs.Add(zanr);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ZanrExists(zanr.ZanrId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetZanr", new { id = zanr.ZanrId }, zanr);
        }

        // DELETE: api/Zanrs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteZanr(int id)
        {
            var zanr = await _context.Zanrs.FindAsync(id);
            if (zanr == null)
            {
                return NotFound();
            }

            _context.Zanrs.Remove(zanr);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ZanrExists(int id)
        {
            return _context.Zanrs.Any(e => e.ZanrId == id);
        }
    }
}
