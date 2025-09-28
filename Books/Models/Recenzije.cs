using System;
using System.Collections.Generic;

namespace Books.Models;

public partial class Recenzije
{
    public int RecenzijaId { get; set; }

    public int? KorisnikId { get; set; }

    public int? KnjigaId { get; set; }

    public int? Ocjena { get; set; }

    public string? Komentar { get; set; }

    public DateTime? DatumRecenzije { get; set; }

    public virtual Knjige? Knjiga { get; set; }

    public virtual ICollection<Komentari> Komentaris { get; set; } = new List<Komentari>();

    public virtual Korisnici? Korisnik { get; set; }
}
