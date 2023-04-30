using System;
using System.Collections.Generic;

namespace mojoin.Models;

public partial class Role
{
    public int RolelD { get; set; }

    public string? RoleName { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
