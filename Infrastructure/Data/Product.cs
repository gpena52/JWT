using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
