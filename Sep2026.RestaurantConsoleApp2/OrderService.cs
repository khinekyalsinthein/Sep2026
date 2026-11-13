using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Sep2026.RestaurantConsoleApp2
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

        private int GetPendingOrderId(SqlConnection connection, SqlTransaction transaction = null)
        {
            SqlCommand cmd = new SqlCommand(
                "SELECT TOP 1 [OrderId] FROM [dbo].[Orders] WHERE [Status] = 'Pending' ORDER BY [OrderId] DESC",
                connection, transaction);
          
            object result = cmd.ExecuteScalar();

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }

        private List<OrderDto> GetCartItems(int orderId, SqlConnection connection)
        {
            string query = $@"
            SELECT OD.[FoodId], F.[FoodName], OD.[FoodPrice], OD.[Quantity]
            FROM [dbo].[OrderDetails] OD
            INNER JOIN [dbo].[Foods] F
            ON OD.[FoodId] = F.[FoodId]
            WHERE OD.[OrderId] = {orderId}";

            SqlCommand cmd = new SqlCommand(query, connection);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();

            adapter.Fill(table);

            List<OrderDto> orders = new List<OrderDto>();

            foreach (DataRow item in table.Rows)
            {
                OrderDto order = new OrderDto
                {
                    FoodId = Convert.ToInt32(item["FoodId"]),
                    FoodName = Convert.ToString(item["FoodName"]) ?? "",
                    FoodPrice = Convert.ToDecimal(item["FoodPrice"]),
                    Quantity = Convert.ToInt32(item["Quantity"])
                };

                orders.Add(order);
            }

            return orders;
        }

        public void AddToCart(int foodId, int quantity)
        {
            if (quantity <= 0)
            {
                Console.WriteLine("Quantity must be greater than zero!");
                return;
            }

            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);

            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            try
            {

                SqlCommand foodCmd = new SqlCommand(
                    $"SELECT [Price] FROM [dbo].[Foods] WHERE [FoodId] = {foodId} AND [IsAvailable] = 1",
                    connection, transaction);

                object result = foodCmd.ExecuteScalar();


                if (result is null)
                {
                    Console.WriteLine("Invalid Food ID!");
                    connection.Close();
                    return;
                }

                decimal price = Convert.ToDecimal(result);

                int orderId = GetPendingOrderId(connection, transaction);

                if (orderId == 0)
                {
                    SqlCommand createOrderCmd = new SqlCommand(
                        "INSERT INTO [dbo].[Orders] ([Status]) VALUES ('Pending'); SELECT SCOPE_IDENTITY();",
                        connection, transaction);

                    orderId = Convert.ToInt32(createOrderCmd.ExecuteScalar());
                }

                SqlCommand checkCmd = new SqlCommand(
                    $"SELECT [OrderDetailId] FROM [dbo].[OrderDetails] WHERE [OrderId] = {orderId} AND [FoodId] = {foodId}",
                    connection, transaction);

                object item = checkCmd.ExecuteScalar();

                if (item != null)
                {
                   
                    SqlCommand updateCmd = new SqlCommand(
                        $"UPDATE [dbo].[OrderDetails] SET [Quantity] = [Quantity] + {quantity} WHERE [OrderId] = {orderId} AND [FoodId] = {foodId}",
                        connection, transaction);

                    updateCmd.ExecuteNonQuery();
                }
                else
                {
                 
                    SqlCommand insertCmd = new SqlCommand(
                        $"INSERT INTO [dbo].[OrderDetails] ([OrderId], [FoodId], [FoodPrice], [Quantity]) VALUES ({orderId}, {foodId}, {price}, {quantity})",
                        connection, transaction);
                    insertCmd.ExecuteNonQuery();
                }
                transaction.Commit();

                Console.WriteLine("Food added to cart successfully!");
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                connection.Close();
            }
           
        }

        public bool ShowCart()
        {
            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);

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
                connection.Close();
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

            connection.Close();

            return true;
        }

        public void RemoveFromCart(int foodId)
        {
            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);

            connection.Open();

            int orderId = GetPendingOrderId(connection);

            if (orderId == 0)
            {
                Console.WriteLine("Your cart is empty.");
                connection.Close();
                return;
            }

            List<OrderDto> orders = GetCartItems(orderId, connection);

            if (orders.Count == 0)
            {
                Console.WriteLine("Your cart is empty.");
                connection.Close();
                return;
            }

            SqlCommand cmd = new SqlCommand(
                $"DELETE FROM [dbo].[OrderDetails] WHERE [FoodId] = {foodId} AND [OrderId] = {orderId}",
                connection);

            int rows = cmd.ExecuteNonQuery();

            if (rows > 0)
            {
                Console.WriteLine("Food removed from cart successfully!");
            }
            else
            {
                Console.WriteLine("Food not found in cart!");
            }

            connection.Close();

        }

        public void CheckOut()
        {
            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);

            connection.Open();

            int orderId = GetPendingOrderId(connection);

            if (orderId == 0)
            {
                Console.WriteLine("Your cart is empty. Please order food first.");
                connection.Close();
                return;
            }

            List<OrderDto> orders = GetCartItems(orderId, connection);

            if (orders.Count == 0)
            {
                Console.WriteLine("Your cart is empty. Please order food first.");
                connection.Close();
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

            SqlCommand updateOrderCmd = new SqlCommand(
                $"UPDATE [dbo].[Orders] SET [Status] = 'Completed' WHERE [OrderId] = {orderId}",
                connection);

            updateOrderCmd.ExecuteNonQuery();

            connection.Close();
        }
    }
}

    

