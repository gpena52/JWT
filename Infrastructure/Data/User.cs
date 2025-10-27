using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Username { get; set; } = null!;

    public int Role { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
