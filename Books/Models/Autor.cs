using System;
using System.Collections.Generic;

namespace Books.Models;

public partial class Autor
{
    public int AutorId { get; set; }

    public string? Ime { get; set; }

    public string? Prezime { get; set; }

    public virtual ICollection<Knjige> Knjiges { get; set; } = new List<Knjige>();
}
