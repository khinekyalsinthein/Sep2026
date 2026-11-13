using System;
using System.Collections.Generic;

namespace Sep2026.ConsoleApp4.Database.AppDbContextModels;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Food> Foods { get; set; } = new List<Food>();
}
