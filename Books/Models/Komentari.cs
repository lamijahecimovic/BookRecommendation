using System;
using System.Collections.Generic;

namespace Books.Models;

public partial class Komentari
{
    public int KomentarId { get; set; }

    public int? RecenzijaId { get; set; }

    public int? KorisnikId { get; set; }

    public string? TekstKomentara { get; set; }

    public DateTime? DatumKomentara { get; set; }

    public virtual Korisnici? Korisnik { get; set; }

    public virtual Recenzije? Recenzija { get; set; }
}
