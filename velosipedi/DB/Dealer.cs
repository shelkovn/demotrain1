using System;
using System.Collections.Generic;

namespace velosipedi.DB;

public partial class Dealer
{
    public int DealerId { get; set; }

    public string DealerName { get; set; } = null!;

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
