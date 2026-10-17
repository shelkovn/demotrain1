using System;
using System.Collections.Generic;

namespace velosipedi.DB;

public partial class PickupPoint
{
    public int PointId { get; set; }

    public string PointAddress { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
