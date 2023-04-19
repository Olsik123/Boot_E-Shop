using System;
using System.Collections.Generic;

namespace WebApplication1;

public partial class TbColor
{
    public int ColorId { get; set; }

    public string Color { get; set; } = null!;

    public string Hexadecimal { get ; set; }

    public virtual ICollection<TbVariation> TbVariations { get; } = new List<TbVariation>();
}
