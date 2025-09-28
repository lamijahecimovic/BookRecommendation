using System;
using System.Collections.Generic;

namespace Books.Models;

public partial class Zanr
{
    public int ZanrId { get; set; }

    public string? NazivZanra { get; set; }

    public virtual ICollection<Knjige> Knjiges { get; set; } = new List<Knjige>();
}
