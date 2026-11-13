using System;
using System.Collections.Generic;

namespace Sep2026.ConsoleApp4.Database.AppDbContextModels;

public partial class Food
{
    public int FoodId { get; set; }

    public string FoodName { get; set; } = null!;

    public decimal Price { get; set; }

    public string Category { get; set; } = null!;

    public bool IsAvailable { get; set; }

    public int CategoryId { get; set; }

    public virtual Category CategoryNavigation { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
