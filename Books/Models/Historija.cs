using System;
using System.Collections.Generic;

namespace Books.Models;

public partial class Historija
{
    public int HistorijaId { get; set; }

    public int? KorisnikId { get; set; }

    public int? KnjigaId { get; set; }

    public virtual Knjige? Knjiga { get; set; }

    public virtual Korisnici? Korisnik { get; set; }
}
