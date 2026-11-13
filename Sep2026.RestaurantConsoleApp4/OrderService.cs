using Microsoft.Data.SqlClient;
using Sep2026.ConsoleApp4.Database.AppDbContextModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Sep2026.RestaurantConsoleApp4
{
    public class OrderService
    {
        private const decimal TaxRate = 0.05m;

        private int GetPendingOrderId(AppDbContext db)
        {
            var order = db.Orders.Where(x => x.Status == "Pending").OrderByDescending(x => x.OrderId).FirstOrDefault();

            if(order is null)
            {
                return 0;
            }
            return order.OrderId;
        }

        private List<OrderDetail> GetCartItems(AppDbContext db, int orderId)
        {
            var items = db.OrderDetails.Where(x => x.OrderId == orderId).ToList();
            return items;
        }
        public void AddToCart(int foodId, int quantity)
        {
            if (quantity <= 0)
            {
                Console.WriteLine("Quantity must be greater than zero!");
                return;
            }

            AppDbContext db = new AppDbContext();
            var transaction = db.Database.BeginTransaction();

                try
                {
                var food = db.Foods.FirstOrDefault(x => x.FoodId == foodId && x.IsAvailable == true);
                  
                if(food is null)
                {
                    Console.WriteLine("Invalid Food ID!");
                    return;
                }
                int orderId = GetPendingOrderId(db);

                if (orderId == 0)
                {
                    Order order = new Order()
                    {
                        OrderDate = DateTime.Now,
                        Status = "Pending"
                    };
                    db.Orders.Add(order);
                    db.SaveChanges();
                    orderId = order.OrderId;
                }

                var item = db.OrderDetails.FirstOrDefault(x => x.OrderId == orderId && x.FoodId == foodId);
                if(item != null)
                {
                    item.Quantity += quantity;
                }
                else
                {
                    OrderDetail orderDetail = new OrderDetail()
                    {
                        OrderId = orderId,
                        FoodId = foodId,
                        FoodPrice = food.Price,
                        Quantity = quantity
                    };
                    db.OrderDetails.Add(orderDetail);
                }
                db.SaveChanges();
                transaction.Commit();
                
                Console.WriteLine("Food added to cart successfully!");
                }
                catch(Exception ex) 
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.ToString());
                }
            finally
            {
                transaction.Dispose();
                db.Dispose();
            }
                
        }

        public bool ShowCart()
        {
            AppDbContext db = new AppDbContext();
            try
            {
                int orderId = GetPendingOrderId(db);

                if (orderId == 0)
                {
                    Console.WriteLine("Your cart is empty.");
                    return false;
                }

                List<OrderDetail> orders = GetCartItems(db, orderId);

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

                decimal subtotal = 0;
                foreach (var item in orders)
                {
                    var food = db.Foods.FirstOrDefault(x => x.FoodId == item.FoodId);

                    if (food != null)
                    {
                        decimal total = item.Quantity * food.Price;
                        subtotal += total;
                        Console.WriteLine($"{food.FoodId,-5} {food.FoodName,-20} {item.Quantity,-5} {food.Price,-10:N0} {total,-10:N0}");

                    }
                }
                Console.WriteLine(new string('-', 50));
                Console.WriteLine($"Subtotal: {subtotal:N0} MMK");
                Console.WriteLine("==================================================");
                return true;
            }
            finally
            {
                db.Dispose();
            }
        }
        public void RemoveFromCart(int foodId)
        {
          AppDbContext db = new AppDbContext();
            try
            {
                int orderId = GetPendingOrderId(db);

                if (orderId == 0)
                {
                    Console.WriteLine("Your cart is empty.");
                    return;
                }
                var item = db.OrderDetails.FirstOrDefault(x => x.OrderId == orderId && x.FoodId == foodId);

                if (item is null)
                {
                    Console.WriteLine("Food not found in cart!");
                    return;

                }

                db.OrderDetails.Remove(item);
                db.SaveChanges();
                Console.WriteLine("Food removed from cart successfully!");
            }
            finally
            {
                db.Dispose();
            }
        }

        public void CheckOut()
        {
            AppDbContext db = new AppDbContext();

            try
            {
                int orderId = GetPendingOrderId(db);

                if (orderId == 0)
                {
                    Console.WriteLine("Your cart is empty. Please order food first.");
                    return;
                }

                List<OrderDetail> orders = GetCartItems(db, orderId);

                if (orders.Count == 0)
                {
                    Console.WriteLine("Your cart is empty. Please order food first.");
                    return;
                }

                decimal subtotal = 0;

                Console.WriteLine("==========================================");
                Console.WriteLine("                 RECEIPT");
                Console.WriteLine("==========================================");

                foreach (OrderDetail item in orders)
                {
                    var food = db.Foods.FirstOrDefault(x => x.FoodId == item.FoodId);

                    if (food != null)
                    {
                        decimal itemTotal = item.FoodPrice * item.Quantity;
                        subtotal += itemTotal;

                        Console.WriteLine($"Item Name: {food.FoodName}");
                        Console.WriteLine($"Qty: {item.Quantity}");
                        Console.WriteLine($"Price: {item.FoodPrice:N0} MMK");
                        Console.WriteLine($"Total: {itemTotal:N0} MMK");
                        Console.WriteLine("------------------------------------------");
                    }
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

                var order = db.Orders.FirstOrDefault(x => x.OrderId == orderId);

                if (order != null)
                {
                    order.Status = "Completed";
                    db.SaveChanges();
                }
            }
            finally
            {
                db.Dispose();
            }
        }
    
    }

}

