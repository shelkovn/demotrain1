using System;
using System.Collections.Generic;

namespace demo1.DB;

public partial class Product
{
    public string Article { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public int Price { get; set; }

    public int ProviderId { get; set; }

    public int ManufacturerId { get; set; }

    public int CategoryId { get; set; }

    public int CurrentDiscount { get; set; }

    public int Count { get; set; }

    public string Description { get; set; } = null!;

    public string? PhotoPath { get; set; }

    public virtual ProductCategory Category { get; set; } = null!;

    public virtual Company Manufacturer { get; set; } = null!;

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual Company Provider { get; set; } = null!;
}
