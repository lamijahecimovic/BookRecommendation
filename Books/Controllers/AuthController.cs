using Books.Models;
using Books.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly KnjigeContext _context;

    public AuthController(KnjigeContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        try
        {
            
            if (request == null)
                return BadRequest(new { Message = "Request ne može biti null." });

            if (string.IsNullOrWhiteSpace(request.KorisnickoIme))
                return BadRequest(new { Message = "Korisničko ime je obavezno." });

            if (string.IsNullOrWhiteSpace(request.Lozinka))
                return BadRequest(new { Message = "Lozinka je obavezna." });

            
            if (await _context.Korisnicis.AnyAsync(k => k.KorisnickoIme == request.KorisnickoIme))
            {
                return BadRequest(new { Message = "Korisničko ime već postoji." });
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Lozinka);

            
            var korisnik = new Korisnici
            {
                Ime = request.Ime,
                Prezime = request.Prezime,
                KorisnickoIme = request.KorisnickoIme,
                Email = request.Email,
                Lozinka = hashedPassword,
                DatumRegistracije = DateTime.Now,
               
            };

           
            _context.Korisnicis.Add(korisnik);
            var affectedRows = await _context.SaveChangesAsync();
            

            Console.WriteLine($"Broj pogođenih redova: {affectedRows}");

            if (affectedRows == 0)
                throw new Exception("Nijedan zapis nije ažuriran");

          

            return Ok(new { Message = "Registracija uspješna.", UserId = korisnik.KorisnikId });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Greška pri registraciji: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"INNER: {ex.InnerException.Message}");
            }

            return StatusCode(500, new
            {
                Message = "Došlo je do greške pri registraciji.",
                Error = ex.Message,
                Inner = ex.InnerException?.Message
            });
        }

    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        try
        {
            var korisnik = await _context.Korisnicis
                .FirstOrDefaultAsync(k => k.KorisnickoIme == request.KorisnickoIme);

            if (korisnik == null || !BCrypt.Net.BCrypt.Verify(request.Lozinka, korisnik.Lozinka))
                return Unauthorized(new { Message = "Pogrešno korisničko ime ili lozinka." });

            return Ok(new
            {
                Message = "Login uspješan.",
                KorisnikId = korisnik.KorisnikId,
                KorisnickoIme = korisnik.KorisnickoIme
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Greška pri prijavi: {ex}");
            return StatusCode(500, new
            {
                Message = "Došlo je do greške pri prijavi.",
                Error = ex.Message
            });
        }
    }
    
    [HttpGet("me")]
    public async Task<IActionResult> GetProfile([FromQuery] string korisnickoIme)
    {
        var korisnik = await _context.Korisnicis
            .FirstOrDefaultAsync(k => k.KorisnickoIme == korisnickoIme);

        if (korisnik == null)
            return NotFound(new { Message = "Korisnik nije pronađen." });

        return Ok(new
        {
            korisnik.KorisnikId,
            korisnik.Ime,
            korisnik.Prezime,
            korisnik.Email
        });
    }


}