using System;
using System.Collections.Generic;

namespace demo1.DB;

public partial class Order
{
    public int OrderId { get; set; }

    public DateOnly OrderedDate { get; set; }

    public DateOnly DeliveryDate { get; set; }

    public int PickUpPointId { get; set; }

    public int UserId { get; set; }

    public int DeliveryCode { get; set; }

    public int StatusId { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual PickUpPoint PickUpPoint { get; set; } = null!;

    public virtual OrderStatus Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
