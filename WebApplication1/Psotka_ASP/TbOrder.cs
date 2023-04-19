using System;
using System.Collections.Generic;

namespace WebApplication1;

public partial class TbOrder
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public DateTime Date { get; set; }

    public decimal Discount { get; set; }

    public decimal Price { get; set; }

    public decimal Dph { get; set; }

    public virtual TbCustomer Customer { get; set; } = null!;

    public virtual ICollection<TbOrderDetail> TbOrderDetails { get; } = new List<TbOrderDetail>();
}