using System;
using System.Collections.Generic;

namespace WebApplication1;

public partial class TbOrderDetail
{
    public int Odid { get; set; }

    public int OrderId { get; set; }

    public int? VariationId { get; set; }

    public string Photo { get; set; } = null!;

    public string Name { get; set; } = null!;

    public decimal Size { get; set; }

    public string Color { get; set; } = null!;

    public decimal Price { get; set; }

    public decimal Dph { get; set; }

    public decimal Discount { get; set; }

    public int Quantity { get; set; }

    public virtual TbOrder Order { get; set; } = null!;

    public virtual TbVariation? Variation { get; set; }
}