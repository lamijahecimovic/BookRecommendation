using Books.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class RecommendationsController : ControllerBase
{
    private readonly RecommenderService _recommender;
    private readonly KnjigeContext _db;

    public RecommendationsController(RecommenderService recommender, KnjigeContext db)
    {
        _recommender = recommender;
        _db = db;
    }

    [HttpGet("{korisnikId}")]
    public IActionResult GetRecommendations(uint korisnikId)
    {
        var korisnikImaOcjene = _db.Recenzijes.Any(r => r.KorisnikId == korisnikId);

        if (!korisnikImaOcjene)
        {
            var popularneKnjige = _db.Knjiges
                .OrderByDescending(k => k.Recenzijes.Count)
                .Take(5)
                .ToList();

            var rezultat = popularneKnjige.Select(k => new
            {
                KnjigaId = k.KnjigaId,
                Naslov = k.Naslov,
                Autor = k.Autor,
                SlikaUrl = k.SlikaUrl,
                PredvidjenaOcjena = 0f
            }).ToList();

            return Ok(rezultat);
        }

        var sveKnjige = _db.Knjiges.ToList();
        var preporuke = new List<(int knjigaId, float score)>();

        foreach (var knjiga in sveKnjige)
        {
            float score = _recommender.PredictRating(korisnikId, (uint)knjiga.KnjigaId);
            preporuke.Add((knjiga.KnjigaId, score));
        }

        var topPreporuke = preporuke
            .Where(p => !float.IsNaN(p.score) && !float.IsInfinity(p.score))
            .OrderByDescending(p => p.score)
            .Take(5)
            .Select(p => new
            {
                KnjigaId = p.knjigaId,
                PredvidjenaOcjena = p.score,
                Knjiga = _db.Knjiges.Find(p.knjigaId)
            })
            .ToList();

        return Ok(topPreporuke);
    }

    [HttpPost("AddReview")]
    public async Task<IActionResult> AddReview([FromBody] RecenzijaDto dto)
    {
        try
        {
            var novaRecenzija = new Recenzije
            {
                KorisnikId = dto.KorisnikId,
                KnjigaId = dto.KnjigaId,
                Ocjena = dto.Ocjena,
                Komentar = dto.Komentar,
            };

            _db.Recenzijes.Add(novaRecenzija);
            await _db.SaveChangesAsync();

            await RegenerisiRatingsCsv();
            _recommender.Treniraj(); 

            return Ok("Recenzija dodana, CSV ažuriran i model treniran.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Greška: " + ex.Message);
        }
    }

    private async Task RegenerisiRatingsCsv()
    {
        var sveRecenzije = _db.Recenzijes.ToList();
        var csvPutanja = Path.Combine(Directory.GetCurrentDirectory(), "MLData", "ratings.csv");

        var sb = new StringBuilder();
        sb.AppendLine("KorisnikId,KnjigaId,Ocjena");

        foreach (var r in sveRecenzije)
        {
            sb.AppendLine($"{r.KorisnikId},{r.KnjigaId},{r.Ocjena}");
        }

        await System.IO.File.WriteAllTextAsync(csvPutanja, sb.ToString());
    }
}
