using System;
using System.Collections.Generic;

namespace velosipedi.DB;

public partial class Item
{
    public int ItemId { get; set; }

    public string ItemArticle { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public double Price { get; set; }

    public int DealerId { get; set; }

    public int ManufacturerId { get; set; }

    public int CategoryId { get; set; }

    public int CurrentDiscount { get; set; }

    public int AmountInStock { get; set; }

    public string ItemDescription { get; set; } = null!;

    public string? ImagePath { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Dealer Dealer { get; set; } = null!;

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
