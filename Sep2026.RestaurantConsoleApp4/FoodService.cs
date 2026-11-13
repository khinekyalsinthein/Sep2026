using Microsoft.Data.SqlClient;
using Sep2026.ConsoleApp4.Database.AppDbContextModels;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Sep2026.RestaurantConsoleApp4
{
    public class FoodService
    {
        public void AddFood(string name, decimal price, string category)
        {
            AppDbContext db = new AppDbContext();

            Food food = new Food
            {
                FoodName = name.Trim(),
                Price = price,
                Category = category.Trim(),
                IsAvailable = true
            };

            db.Foods.Add(food);
            db.SaveChanges();
            Console.WriteLine("Food added successfully.");
        }

        public bool ShowFoods()
        {
            AppDbContext db = new AppDbContext();
            try
            {
                var foods = db.Foods.Where(x => x.IsAvailable == true).OrderBy(x => x.Category).ThenBy(x => x.FoodId).ToList();

                if (foods.Count == 0)
                {
                    Console.WriteLine("No food available.");
                    return false;
                }
                Console.WriteLine("==========================================");
                Console.WriteLine("              RESTAURANT MENU");
                Console.WriteLine("==========================================");

                string currentCategory = "";

                foreach (Food food in foods)
                {
                    if (currentCategory != food.Category)
                    {
                        currentCategory = food.Category;

                        Console.WriteLine($"\n----------- {currentCategory.ToUpper()} -----------");
                        Console.WriteLine($"{"ID",-5} {"Food Name",-25} {"Price",-12}");
                    }

                    Console.WriteLine($"{food.FoodId,-5} {food.FoodName,-25} {food.Price:N0} MMK");
                }

                Console.WriteLine("==========================================");

                return true;
            }
            finally
            {
                db.Dispose();
            }
        }
    
    public void UpdateFood(int foodId, string foodName, decimal price, string category)
        {
            AppDbContext db = new AppDbContext();
            var food = db.Foods.FirstOrDefault(x => x.FoodId == foodId && x.IsAvailable == true);

            if (food is null)
            {
                Console.WriteLine("Food not found.");
                return;
            }

            food.FoodName = foodName.Trim();
            food.Price = price;
            food.Category = category.Trim();

            db.SaveChanges();
            Console.WriteLine("Food updated successfully.");
        }

        public void DeleteFood(int foodId)
        {
            AppDbContext db = new AppDbContext();
            var food = db.Foods.FirstOrDefault(x => x.FoodId == foodId && x.IsAvailable == true);


            if (food is null)
            {
                Console.WriteLine("Food not found.");
                return;
            }

            food.IsAvailable = false;
            db.SaveChanges();
            Console.WriteLine("Food deleted successfully.");
        }
           
        }
    }


    

