using System;
using System.Collections.Generic;

namespace WebApplication1;

public partial class TbBoot
{
    public int BootId { get; set; }

    public string Name { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;

    public string LongDescription { get; set; } = null!;

    public string Material { get; set; } = null!;

    public int Code { get; set; }

    public virtual ICollection<TbBootCategory> TbBootCategories { get; set; } = new List<TbBootCategory>();

    public virtual ICollection<TbPhoto> TbPhotos { get; set; } = new List<TbPhoto>();

    public virtual ICollection<TbVariation> TbVariations { get; set; } = new List<TbVariation>();
}