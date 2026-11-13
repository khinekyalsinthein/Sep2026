using System;
using System.Collections.Generic;

namespace Sep2026.ConsoleApp4.Database.AppDbContextModels;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int OrderId { get; set; }

    public int FoodId { get; set; }

    public int Quantity { get; set; }

    public decimal FoodPrice { get; set; }

    public virtual Food Food { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
