using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sep2026.RestaurantConsoleApp3
{
    public class FoodDto
    {
        public int FoodId { get; set; }
        public string FoodName { get; set; } = "";
        public decimal Price { get; set; }
        public string Category { get; set; } = "";

    }
}
