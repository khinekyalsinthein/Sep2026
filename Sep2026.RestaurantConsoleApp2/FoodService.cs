using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sep2026.RestaurantConsoleApp2
{
    public class FoodService
    {
        SqlConnectionStringBuilder sqlConnectionStringBuilder =  new SqlConnectionStringBuilder()
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

            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);

            connection.Open();

            SqlCommand command = new SqlCommand(
                $"INSERT INTO [dbo].[Foods] ([FoodName], [Price], [Category]) VALUES ('{name}', {price}, '{category}')",
                connection);

            command.ExecuteNonQuery();

            Console.WriteLine("Food added successfully.");

            connection.Close();
        }

        public bool ShowFoods()
        {
            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);

            connection.Open();

            SqlCommand cmd = new SqlCommand(
                @"SELECT [FoodId], [FoodName], [Price], [Category]
                FROM [dbo].[Foods]  
                WHERE [IsAvailable] = 1
                ORDER BY [Category], [FoodId]",
                connection);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();

            adapter.Fill(table);

            if (table.Rows.Count == 0)
            {
                Console.WriteLine("No food available.");
                connection.Close();
                return false;
            }

            List<FoodDto> foods = new List<FoodDto>();

            foreach (DataRow item in table.Rows)
            {
                FoodDto food = new FoodDto
                {
                    FoodId = Convert.ToInt32(item["FoodId"]),
                    FoodName = Convert.ToString(item["FoodName"]) ?? "",
                    Price = Convert.ToDecimal(item["Price"]),
                    Category = Convert.ToString(item["Category"]) ?? ""
                };

                foods.Add(food);
            }

            Console.WriteLine("==========================================");
            Console.WriteLine("              RESTAURANT MENU");
            Console.WriteLine("==========================================");

            string currentCategory = "";

            foreach (FoodDto food in foods)
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

            connection.Close();
            return true;
        }

        public void UpdateFood(int foodId, string foodName, decimal price, string category)
        {
            foodName = foodName.Trim();
            category = category.Trim();

            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);

            connection.Open();

            SqlCommand cmd = new SqlCommand(
                $"UPDATE [dbo].[Foods] SET [FoodName] = '{foodName}', [Price] = {price}, [Category] = '{category}' WHERE [FoodId] = {foodId}",
                connection);

            int rows = cmd.ExecuteNonQuery();

            if (rows > 0)
            {
                Console.WriteLine("Food updated successfully.");
            }
            else
            {
                Console.WriteLine("Food not found.");
            }

            connection.Close();
        }

        public void DeleteFood(int foodId)
        {
            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);

            connection.Open();

            SqlCommand cmd = new SqlCommand(
                $"UPDATE [dbo].[Foods] SET [IsAvailable] = 0 WHERE [FoodId] = {foodId} AND [IsAvailable] = 1",
                connection);
            int rows = cmd.ExecuteNonQuery();

            if (rows > 0)
            {
                Console.WriteLine("Food deleted successfully.");
            }
            else
            {
                Console.WriteLine("Food not found.");
            }

            connection.Close();
        }
    }
}
    
       