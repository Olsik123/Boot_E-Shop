using System;
using System.Collections.Generic;

namespace WebApplication1;

public partial class TbCategory
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public int Left { get; set; }

    public int Right { get; set; }

    public virtual ICollection<TbBootCategory> TbBootCategories { get; } = new List<TbBootCategory>();
}
