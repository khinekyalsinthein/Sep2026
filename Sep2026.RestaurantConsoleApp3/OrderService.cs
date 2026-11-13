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
    public class OrderService
    {
        SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
        {
            DataSource = ".",
            InitialCatalog = "ProjectDb",
            UserID = "sa",
            Password = "sasa@123",
            TrustServerCertificate = true
        };

        private const decimal TaxRate = 0.05m;

        private int GetPendingOrderId(IDbConnection connection, IDbTransaction transaction = null)
        {
           
            string query = @"SELECT TOP 1 [OrderId] 
                    FROM [dbo].[Orders] 
                    WHERE [Status] = 'Pending' 
                    ORDER BY [OrderId] DESC";

            return connection.QueryFirstOrDefault<int>(query, transaction: transaction);
           
        }

        private List<OrderDto> GetCartItems(int orderId, IDbConnection connection)
        {
            
            string query = $@"
            SELECT OD.[FoodId], F.[FoodName], OD.[FoodPrice], OD.[Quantity]
            FROM [dbo].[OrderDetails] OD
            INNER JOIN [dbo].[Foods] F
            ON OD.[FoodId] = F.[FoodId]
            WHERE OD.[OrderId] = {orderId}";

            return connection.Query<OrderDto>(query, new { OrderId = orderId }).ToList();
           
        }

        public void AddToCart(int foodId, int quantity)
        {
            if (quantity <= 0)
            {
                Console.WriteLine("Quantity must be greater than zero!");
                return;
            }

            using IDbConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
            connection.Open();
            string foodquery = $@"SELECT [Price] 
                        FROM [dbo].[Foods] 
                        WHERE [FoodId] = {foodId} AND [IsAvailable] = 1";
            decimal? price = connection.QueryFirstOrDefault<decimal?>(foodquery, new { FoodId = foodId });

            if (price is null)
            {
                Console.WriteLine("Invalid Food ID!");
               
                return;
            }

            using (var transaction = connection.BeginTransaction())
            {
                try
                {

                    int orderId = GetPendingOrderId(connection, transaction);
                    if (orderId == 0)
                    {
                        string orderquery = @"INSERT INTO [dbo].[Orders] ([Status]) 
                    VALUES ('Pending'); 
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        orderId = connection.ExecuteScalar<int>(orderquery, transaction: transaction);

                    }

                    string checkQuery = $@"SELECT [OrderDetailId] 
                        FROM [dbo].[OrderDetails] 
                        WHERE [OrderId] = {orderId} AND [FoodId] = {foodId}";

                    int item = connection.QueryFirstOrDefault<int>(checkQuery, new { OrderId = orderId, FoodId = foodId }, transaction);


                    if (item > 0)
                    {
                        
                        string updateQuery = $@"UPDATE [dbo].[OrderDetails] 
                            SET [Quantity] = [Quantity] + {quantity} 
                            WHERE [OrderId] = {orderId} AND [FoodId] = {foodId}";

                        connection.Execute(updateQuery, new { OrderId = orderId, FoodId = foodId, Quantity = quantity }, transaction);

                    }
                    else
                    {
                       
                        string insertQuery = $@"INSERT INTO [dbo].[OrderDetails] ([OrderId], [FoodId], [FoodPrice], [Quantity]) 
                            VALUES ({orderId}, {foodId}, {price}, {quantity})";
                        connection.Execute(insertQuery, new { OrderId = orderId, FoodId = foodId, FoodPrice = price, Quantity = quantity }, transaction);

                    }
                    transaction.Commit();
                    Console.WriteLine("Food added to cart successfully!");
                }
                catch {
                    transaction.Rollback();
                    throw;
                }
               
            }
        }

        
        public bool ShowCart()
        {
            using IDbConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            int orderId = GetPendingOrderId(connection);

            if (orderId == 0)
            {
                Console.WriteLine("Your cart is empty.");
                connection.Close();
                return false;
            }

            List<OrderDto> orders = GetCartItems(orderId, connection);

            if (orders.Count == 0)
            {
                Console.WriteLine("Your cart is empty.");         
                return false;
            }

            Console.WriteLine("==================================================");
            Console.WriteLine("              YOUR CURRENT CART");
            Console.WriteLine("==================================================");
            Console.WriteLine($"{"ID",-5} {"Item Name",-20} {"Qty",-5} {"Price",-10} {"Total",-10}");
            Console.WriteLine(new string('-', 50));

            foreach (OrderDto item in orders)
            {
                Console.WriteLine(
                    $"{item.FoodId,-5} {item.FoodName,-20} {item.Quantity,-5} {item.FoodPrice,-10:N0} {item.Total,-10:N0}");
            }

            Console.WriteLine("==================================================");
            return true;
        }

        public void RemoveFromCart(int foodId)
        {
            using IDbConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            int orderId = GetPendingOrderId(connection);

            if (orderId == 0)
            {
                Console.WriteLine("Your cart is empty.");
                return;
             
            }

            List<OrderDto> orders = GetCartItems(orderId, connection);

            if (orders.Count == 0)
            {
                Console.WriteLine("Your cart is empty.");
                return;
              
            }

            string query = $@"DELETE FROM [dbo].[OrderDetails] 
                        WHERE [FoodId] = {foodId} AND [OrderId] = {orderId}";
            int result = connection.Execute(query, new { FoodId = foodId, OrderId = orderId });
           

            if (result > 0)
            {
                Console.WriteLine("Food removed from cart successfully!");
            }
            else
            {
                Console.WriteLine("Food not found in cart!");
            }

        }
        public void CheckOut()
        {
            using IDbConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            int orderId = GetPendingOrderId(connection);

            if (orderId == 0)
            {
                Console.WriteLine("Your cart is empty. Please order food first.");            
                return;
            }

            List<OrderDto> orders = GetCartItems(orderId, connection);

            if (orders.Count == 0)
            {
                Console.WriteLine("Your cart is empty. Please order food first.");
                return;
            }

            decimal subtotal = 0;

            Console.WriteLine("==========================================");
            Console.WriteLine("                 RECEIPT");
            Console.WriteLine("==========================================");

            foreach (OrderDto item in orders)
            {
                subtotal += item.Total;

                Console.WriteLine($"Item Name: {item.FoodName}");
                Console.WriteLine($"Qty: {item.Quantity}");
                Console.WriteLine($"Price: {item.FoodPrice:N0} MMK");
                Console.WriteLine($"Total: {item.Total:N0} MMK");
                Console.WriteLine("------------------------------------------");
            }

            decimal tax = subtotal * TaxRate;
            decimal total = subtotal + tax;
            Console.WriteLine($"SubTotal: {subtotal:N0} MMK");
            Console.WriteLine($"Tax (5%): {tax:N0} MMK");
            Console.WriteLine("==========================================");
            Console.WriteLine($"TOTAL: {total:N0} MMK");
            Console.WriteLine("==========================================");
            Console.WriteLine("   Thank You For Your Order!");
            Console.WriteLine("==========================================");

            string query = $@"UPDATE [dbo].[Orders] 
                        SET [Status] = 'Completed' 
                        WHERE [OrderId] = {orderId}";
            connection.Execute(query, new { OrderId = orderId }); ;
     
        }
    }
}
   

