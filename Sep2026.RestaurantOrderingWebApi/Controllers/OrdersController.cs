using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sep2026.ConsoleApp4.Database.AppDbContextModels;
using Sep2026.RestaurantOrderingWebApi.Models;

namespace Sep2026.RestaurantOrderingWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
            private readonly AppDbContext _db;

            public OrdersController()
            {
                _db = new AppDbContext();
            }
          
            [HttpGet]
            public IActionResult GetOrders()
            {
                var orders = _db.Orders
                    .Select(x => new
                    {
                        x.OrderId,
                        x.OrderDate,
                        x.Status,

                        Items = _db.OrderDetails
                            .Where(d => d.OrderId == x.OrderId)
                            .Select(d => new
                            {
                                d.OrderDetailId,
                                d.FoodId,
                                FoodName = d.Food.FoodName,
                                d.FoodPrice,
                                d.Quantity,
                                Total = d.FoodPrice * d.Quantity
                            })
                            .ToList()
                    })
                    .ToList();

                return Ok(orders);
            }
           
            [HttpGet("{id}")]
            public IActionResult GetOrder(int id)
            {
                var order = _db.Orders.FirstOrDefault(x =>
                    x.OrderId == id);

                if (order == null)
                {
                    return NotFound("Order not found.");
                }

                var items = _db.OrderDetails
                    .Where(x => x.OrderId == id)
                    .Select(x => new
                    {
                        x.OrderDetailId,
                        x.FoodId,
                        x.Food.FoodName,
                        x.FoodPrice,
                        x.Quantity,
                        Subtotal = x.FoodPrice * x.Quantity
                    })
                    .ToList();

                var result = new
                {
                    order.OrderId,
                    order.OrderDate,
                    order.Status,
                    Items = items
                };

                return Ok(result);
            }
           
            [HttpPost]
            public IActionResult CreateOrder(OrderCreateRequestModel requestModel)
            {
                if (requestModel.Items == null ||
                    requestModel.Items.Count == 0)
                {
                    return BadRequest("Order must contain at least one food item.");
                }
               
                foreach (var item in requestModel.Items)
                {
                    if (item.FoodId <= 0 || item.Quantity <= 0)
                    {
                        return BadRequest("Invalid food ID or quantity.");
                    }

                    var food = _db.Foods.FirstOrDefault(x =>
                        x.FoodId == item.FoodId &&
                        x.IsAvailable == true);

                    if (food == null)
                    {
                        return BadRequest("Food not found or unavailable.");
                    }
                }
                
                var order = new Order
                {
                    OrderDate = DateTime.Now,
                    Status = "Pending"
                };

                _db.Orders.Add(order);
                _db.SaveChanges();

                foreach (var item in requestModel.Items)
                {
                    var food = _db.Foods.FirstOrDefault(x =>
                        x.FoodId == item.FoodId &&
                        x.IsAvailable == true);

                    _db.OrderDetails.Add(new OrderDetail
                    {
                        OrderId = order.OrderId,
                        FoodId = food.FoodId,
                        FoodPrice = food.Price,
                        Quantity = item.Quantity
                    });
                }

                int result = _db.SaveChanges();
                if (result <= 0)
                {
                    return BadRequest("Failed to create order.");
                }

                return Ok(new
                {
                    Message = "Order created successfully.",
                    order.OrderId
                });
            }
           
            [HttpPatch("{id}/items/{detailId}")]
            public IActionResult UpdateOrder(
                int id,
                int detailId,
                OrderUpdateRequestModel requestModel)
            {
                if (!requestModel.FoodId.HasValue &&
                    !requestModel.Quantity.HasValue)
                {
                    return BadRequest("Please provide FoodId or Quantity.");
                }

                var order = _db.Orders.FirstOrDefault(x =>
                    x.OrderId == id &&
                    x.Status == "Pending");

                if (order == null)
                {
                    return NotFound("Pending order not found.");
                }

                var item = _db.OrderDetails.FirstOrDefault(x =>
                    x.OrderId == id &&
                    x.OrderDetailId == detailId);

                if (item == null)
                {
                    return NotFound("Order item not found.");
                }

                if (requestModel.FoodId.HasValue)
                {
                    if (requestModel.FoodId.Value <= 0)
                    {
                        return BadRequest("Invalid FoodId.");
                    }

                    var food = _db.Foods.FirstOrDefault(x =>
                        x.FoodId == requestModel.FoodId.Value &&
                        x.IsAvailable == true);

                    if (food == null)
                    {
                        return NotFound("Food not found or unavailable.");
                    }

                    item.FoodId = food.FoodId;
                    item.FoodPrice = food.Price;
                }

                if (requestModel.Quantity.HasValue)
                {
                    if (requestModel.Quantity.Value <= 0)
                    {
                        return BadRequest("Quantity must be greater than zero.");
                    }

                    item.Quantity = requestModel.Quantity.Value;
                }

                int result = _db.SaveChanges();

                string message = result > 0
                    ? "Order updated successfully."
                    : "Failed to update order.";

                return Ok(message);
            }
        }
    }

