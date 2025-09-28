using System;
using System.Collections.Generic;

namespace Books.Models;

public partial class Korisnici
{
    public int KorisnikId { get; set; }

    public string? KorisnickoIme { get; set; }

    public string? Email { get; set; }

    public string? Lozinka { get; set; }

    public DateTime? DatumRegistracije { get; set; }

    public string? Ime { get; set; }

    public string? Prezime { get; set; }

    public virtual ICollection<Favoriti> Favoritis { get; set; } = new List<Favoriti>();

    public virtual ICollection<Historija> Historijas { get; set; } = new List<Historija>();

    public virtual ICollection<Knjige> Knjiges { get; set; } = new List<Knjige>();

    public virtual ICollection<Komentari> Komentaris { get; set; } = new List<Komentari>();

    public virtual ICollection<Preporuka> Preporukas { get; set; } = new List<Preporuka>();

    public virtual ICollection<Recenzije> Recenzijes { get; set; } = new List<Recenzije>();
}
