using System;
using System.Collections.Generic;

namespace Sep2026.ConsoleApp4.Database.AppDbContextModels;

public partial class Order
{
    public int OrderId { get; set; }

    public DateTime? OrderDate { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
