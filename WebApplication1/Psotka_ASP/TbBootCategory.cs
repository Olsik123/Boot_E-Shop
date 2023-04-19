using System;
using System.Collections.Generic;

namespace WebApplication1;

public partial class TbBootCategory
{
    public int Id { get; set; }

    public int BootId { get; set; }

    public int CategoryId { get; set; }

    public virtual TbBoot Boot { get; set; } = null!;

    public virtual TbCategory Category { get; set; } = null!;
}
