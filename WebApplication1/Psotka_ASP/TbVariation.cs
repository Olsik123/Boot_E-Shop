using System;
using System.Collections.Generic;

namespace WebApplication1;

public partial class TbVariation
{
    public int VariationId { get; set; }

    public int BootId { get; set; }

    public decimal Size { get; set; }

    public int ColorId { get; set; }

    public decimal Price { get; set; }

    public decimal Dph { get; set; }

    public decimal Discount { get; set; }

    public int InStock { get; set; }

    public virtual TbBoot Boot { get; set; } = null!;

    public virtual TbColor Color { get; set; } = null!;

    public virtual ICollection<TbOrderDetail> TbOrderDetails { get; } = new List<TbOrderDetail>();
}