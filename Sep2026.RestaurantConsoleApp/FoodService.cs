using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sep2026.RestaurantConsoleApp
{
    public class FoodService
    {
        public static List<FoodDto> Menus = new List<FoodDto>(){
          new FoodDto { Id = 1, Name ="Spring Rolls", Price = 3500, Category = "Appetizer" },
          new FoodDto { Id = 2, Name = "Chicken Wings", Price = 5500, Category = "Appetizer" },
          new FoodDto { Id = 3, Name = "Fried Rice", Price = 4500, Category = "Main Course" },
          new FoodDto { Id = 4, Name = "Chicken Curry", Price = 6000, Category = "Main Course" },
          new FoodDto { Id = 5, Name = "Beef Burger", Price = 7500, Category = "Main Course" },
          new FoodDto { Id = 6, Name = "Chicken Noodles", Price = 7000, Category = "Main Course" },
          new FoodDto { Id = 7, Name = "Cola", Price = 1500, Category = "Beverage" },
          new FoodDto { Id = 8, Name = "Lemon Juice", Price = 2500, Category = "Beverage" },
          new FoodDto { Id = 9, Name = "Ice Tea", Price = 2500, Category = "Beverage" }

         };

        public void DisplayMenu()
        {
            Console.WriteLine("==========================================");
            Console.WriteLine("              RESTAURANT MENU             ");
            Console.WriteLine("==========================================");

            string[] categories = { "Appetizer", "Main Course", "Beverage" };

            foreach (string category in categories)
            {
                Console.WriteLine($"\n--- {category.ToUpper()} ---");
                foreach (var food in Menus)
                {
                    if (food.Category == category)
                    {
                        Console.WriteLine($"[{food.Id}] {food.Name} - {food.Price:N0} MMK");
                    }

                }
            }
            Console.WriteLine("------------------------------------------");
        }

    }
}
