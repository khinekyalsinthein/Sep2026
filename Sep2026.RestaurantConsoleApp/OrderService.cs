using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sep2026.RestaurantConsoleApp
{
    public class OrderService
    {
        private List<OrderDto> Orders = new List<OrderDto>();

        private const decimal TaxRate = 0.05m;
        public void AddToCart(int foodId, int quantity)
        {

            FoodDto food = FoodService.Menus.Where(x => x.Id == foodId).FirstOrDefault();

            if (food == null)
            {
                Console.WriteLine("Invalid Food ID!");
                return;
            }
            if (quantity <= 0)
            {
                Console.WriteLine("Quantity must be greater than zero!");
                return;
            }
            OrderDto item = Orders.Where(x => x.FoodId == foodId).FirstOrDefault();
            if (item != null)
            {
                item.Quantity += quantity;
            }
            else
            {
                OrderDto newOrder = new OrderDto
                {
                    FoodId = food.Id,
                    FoodName = food.Name,
                    FoodPrice = food.Price,
                    Quantity = quantity
                };

                Orders.Add(newOrder);
            }

            Console.WriteLine($"{food.Name} added to cart successfully!");
            Console.WriteLine("==========================================");
        }

        public void DisplayCart()
        {
            if (Orders.Count == 0)
            {
                Console.WriteLine("Your cart is empty.");
                return;
            }
            Console.WriteLine("==================================================");
            Console.WriteLine("              YOUR CURRENT CART             ");
            Console.WriteLine("==================================================");

            Console.WriteLine($"{"ID",-5} {"Item Name",-20} {"Qty",-5} {"Price",-10} {"Total",-10}");
            Console.WriteLine(new string('-', 50));
            foreach (var item in Orders)
            {
                Console.WriteLine($"{item.FoodId,-5} {item.FoodName,-20} {item.Quantity,-5} {item.FoodPrice,-10:N0} {item.Total,-10:N0}");
            }
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"Subtotal: {CalculateSubtotal():N0} MMK");
            Console.WriteLine("==================================================");

        }
        public bool IsCartEmpty
        {
            get { return Orders.Count == 0; }
        }
        public void RemoveFromCart(int foodId)
        {
            OrderDto item = Orders.Where(x => x.FoodId == foodId).FirstOrDefault();
            if (item != null)
            {
                Orders.Remove(item);

                Console.WriteLine($"{item.FoodName} removed from cart successfully!");
            }
            else
            {
                Console.WriteLine("Food not found in cart!");
            }
            Console.WriteLine("==================================================");
        }
        private decimal CalculateSubtotal()
        {
            decimal subtotal = 0;
            foreach (var item in Orders)
            {
                subtotal += item.Total;
            }
            return subtotal;
        }

        public void CheckOut()
        {
            if (Orders.Count == 0)
            {
                Console.WriteLine("Your cart is empty.Please order food first.");
                return;
            }

            decimal subtotal = CalculateSubtotal();
            decimal tax = subtotal * TaxRate;
            decimal total = subtotal + tax;
            Console.WriteLine("==========================================");
            Console.WriteLine("               RECEIPT");
            Console.WriteLine("==========================================");

            foreach (var item in Orders)
            {
                Console.WriteLine($"Item Name: {item.FoodName}\nQty: {item.Quantity}\nPrice: {item.FoodPrice:N0} MMK\nTotal: {item.Total:N0} MMK");

            }
            Console.WriteLine("------------------------------------------");
            Console.WriteLine($"SubTotal: {subtotal:N0} MMK");
            Console.WriteLine($"Tax: {tax:N0} MMK");
            Console.WriteLine("==========================================");
            Console.WriteLine($"TOTAL: {total:N0} MMK");
            Console.WriteLine("==========================================");
            Console.WriteLine("   Thank You For Your Order!");
            Console.WriteLine("==========================================");


        }
    }
}
