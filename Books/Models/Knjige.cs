using System;
using System.Collections.Generic;

namespace Books.Models;

public partial class Knjige
{
    public int KnjigaId { get; set; }

    public string? Naslov { get; set; }

    public string? Autor { get; set; }

    public string? Opis { get; set; }

    public int? ZanrId { get; set; }

    public int? AutorId { get; set; }

    public int? KorisnikId { get; set; }

    public string? SlikaUrl { get; set; }

    public string? PdfUrl { get; set; }

    public virtual Autor? AutorNavigation { get; set; }

    public virtual ICollection<Favoriti> Favoritis { get; set; } = new List<Favoriti>();

    public virtual ICollection<Historija> Historijas { get; set; } = new List<Historija>();

    public virtual Korisnici? Korisnik { get; set; }

    public virtual ICollection<Preporuka> Preporukas { get; set; } = new List<Preporuka>();

    public virtual ICollection<Recenzije> Recenzijes { get; set; } = new List<Recenzije>();

    public virtual Zanr? Zanr { get; set; }
}
