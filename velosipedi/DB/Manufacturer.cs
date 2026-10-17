using System;
using System.Collections.Generic;

namespace velosipedi.DB;

public partial class Manufacturer
{
    public int ManufacturerId { get; set; }

    public string ManufacturerName { get; set; } = null!;

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
