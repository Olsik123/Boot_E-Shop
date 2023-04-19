using System;
using System.Collections.Generic;

namespace WebApplication1;

public partial class TbCustomer
{
    public int CustomerId { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string? Adress { get; set; }

    public string Country { get; set; } = null!;

    public int PostalCode { get; set; }

    public string Email { get; set; } = null!;

    public int Phone { get; set; }

    public virtual ICollection<TbOrder> TbOrders { get; } = new List<TbOrder>();
}