using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sep2026.RestaurantConsoleApp3
{
    public class FoodService
    {
        SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
        {
            DataSource = ".",
            InitialCatalog = "ProjectDb",
            UserID = "sa",
            Password = "sasa@123",
            TrustServerCertificate = true
        };
        public void AddFood(string name, decimal price, string category)
        {
            name = name.Trim();
            category = category.Trim();

            using IDbConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
            connection.Open();
            string query = $@"INSERT INTO [dbo].[Foods] ([FoodName], [Price], [Category]) 
                    VALUES ('{name}', {price}, '{category}')";
            int result = connection.Execute(query, new
            {
                FoodName = name,
                Price = price,
                Category = category
            });

            Console.WriteLine("Food added successfully.");

        }

        public bool ShowFoods()
        {

            using IDbConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
            connection.Open();
            string query = @"SELECT [FoodId], [FoodName], [Price], [Category] 
                    FROM [dbo].[Foods] 
                    WHERE [IsAvailable] = 1
                    ORDER BY [Category], [FoodId]";
            var lst = connection.Query<FoodDto>(query).ToList();
           

            

            if (lst.Count == 0)
            {
                Console.WriteLine("No food available.");
         
                return false;
            }


            Console.WriteLine("==========================================");
            Console.WriteLine("              RESTAURANT MENU");
            Console.WriteLine("==========================================");

            string currentCategory = "";

            foreach (FoodDto food in lst)
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

        public void UpdateFood(int foodId, string foodName, decimal price, string category)
        {
            foodName = foodName.Trim();
            category = category.Trim();

            using IDbConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
            connection.Open();
            string query = $@"UPDATE [dbo].[Foods] SET [FoodName] = '{foodName}',
                    [Price] = {price}, 
                    [Category] = '{category}' 
                    WHERE [FoodId] = {foodId}";

            int rows = connection.Execute(query, new
            {
                FoodId = foodId,
                FoodName = foodName,
                Price = price,
                Category = category
            });
           
            if (rows > 0)
            {
                Console.WriteLine("Food updated successfully.");
            }
            else
            {
                Console.WriteLine("Food not found.");
            }

        }

        public void DeleteFood(int foodId)
        {
         
            using IDbConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
            connection.Open();
            
             string query = $@"UPDATE [dbo].[Foods] SET [IsAvailable] = 0 WHERE [FoodId] = {foodId}";

            int rows = connection.Execute(query, new { FoodID = foodId });
         

            if (rows > 0)
            {
                Console.WriteLine("Food deleted successfully.");
            }
            else
            {
                Console.WriteLine("Food not found.");
            }

        }
    }
}
    
