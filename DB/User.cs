using System;
using System.Collections.Generic;

namespace demo1.DB;

public partial class User
{
    public int Id { get; set; }

    public int? RoleId { get; set; }

    public string FullName { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role? Role { get; set; }
}
