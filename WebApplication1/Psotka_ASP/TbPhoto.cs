using System;
using System.Collections.Generic;

namespace WebApplication1;

public partial class TbPhoto
{
    public int PhotoId { get; set; }

    public int BootId { get; set; }

    public string Path { get; set; } = null!;

    public virtual TbBoot Boot { get; set; } = null!;
}
