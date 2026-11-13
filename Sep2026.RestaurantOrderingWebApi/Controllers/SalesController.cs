using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sep2026.ConsoleApp4.Database.AppDbContextModels;
using Sep2026.RestaurantOrderingWebApi.Models;

namespace Sep2026.RestaurantOrderingWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
            private readonly AppDbContext _db;
            private const decimal TaxRate = 0.05m;

            public SalesController()
            {
                _db = new AppDbContext();
            }

            private List<CartItemModel> GetSaleItems(int orderId)
            {
                var items = _db.OrderDetails
                    .Where(x => x.OrderId == orderId)
                    .Select(x => new CartItemModel
                    {
                        FoodId = x.FoodId,
                        FoodName = x.Food.FoodName,
                        Quantity = x.Quantity,
                        FoodPrice = x.FoodPrice,
                        Total = x.FoodPrice * x.Quantity
                    })
                    .ToList();

                return items;
            }

            private ReceiptModel GetReceipt(Order order)
            {
                var items = GetSaleItems(order.OrderId);

                decimal subtotal = 0;

                foreach (var item in items)
                {
                    subtotal += item.Total;
                }

                decimal tax = subtotal * TaxRate;
                decimal total = subtotal + tax;

                return new ReceiptModel
                {
                    OrderId = order.OrderId,
                    Items = items,
                    Subtotal = subtotal,
                    Tax = tax,
                    Total = total,
                    Message = "Thank You For Your Order!"
                };
            }

            [HttpGet]
            public IActionResult GetSales()
            {
                var orders = _db.Orders
                    .Where(x => x.Status == "Completed")
                    .OrderByDescending(x => x.OrderId)
                    .ToList();

                var sales = new List<ReceiptModel>();

                foreach (var order in orders)
                {
                    sales.Add(GetReceipt(order));
                }

                return Ok(sales);
            }

            [HttpGet("{id}")]
            public IActionResult GetSale(int id)
            {
                var order = _db.Orders.FirstOrDefault(x =>
                    x.OrderId == id && x.Status == "Completed");

                if (order is null)
                {
                    return NotFound("Sale not found.");
                }

                var receipt = GetReceipt(order);

                return Ok(receipt);
            }

            [HttpPost]
            public IActionResult Checkout(CheckoutRequestModel requestModel)
            {
                var order = _db.Orders.FirstOrDefault(x =>
                    x.OrderId == requestModel.OrderId && x.Status == "Pending");

                if (order is null)
                {
                    return NotFound("Pending order not found.");
                }

                var items = GetSaleItems(order.OrderId);

                if (items.Count == 0)
                {
                    return BadRequest("Order is empty.");
                }

                decimal subtotal = 0;

                foreach (var item in items)
                {
                    subtotal += item.Total;
                }

                decimal tax = subtotal * TaxRate;
                decimal total = subtotal + tax;

                order.Status = "Completed";

                int result = _db.SaveChanges();

                if (result <= 0)
                {
                    return BadRequest("Checkout failed.");
                }

                var receipt = new ReceiptModel
                {
                    OrderId = order.OrderId,
                    Items = items,
                    Subtotal = subtotal,
                    Tax = tax,
                    Total = total,
                    Message = "Thank You For Your Order!"
                };

                return Ok(receipt);
            }
        }
    }

