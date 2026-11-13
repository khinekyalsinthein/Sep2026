using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sep2026.ConsoleApp4.Database.AppDbContextModels;
using Sep2026.RestaurantOrderingWebApi.Models;

namespace Sep2026.RestaurantOrderingWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodsController : ControllerBase
    {
            private readonly AppDbContext _db;

            public FoodsController()
            {
                _db = new AppDbContext();
            }

            [HttpGet]
            public IActionResult GetFoods()
            {
                var foods = _db.Foods.Where(x => x.IsAvailable == true).OrderBy(x => x.CategoryId).ThenBy(x => x.FoodId).ToList();
                return Ok(foods);
            }

            [HttpGet("{id}")]
            public IActionResult GetFood(int id)
            {
                var food = _db.Foods.FirstOrDefault(x => x.FoodId == id && x.IsAvailable == true);
                if (food is null)
                {
                    return NotFound("Food not found");
                }
                return Ok(food);
            }

            [HttpPost]
            public IActionResult AddFood(FoodCreateRequestModel requestModel)
            {
                if (string.IsNullOrWhiteSpace(requestModel.FoodName) ||
                    requestModel.CategoryId <= 0 ||
                    requestModel.Price <= 0)
                {
                    return BadRequest("Invalid food information.");
                }

                var category = _db.Categories.FirstOrDefault(x =>
                    x.CategoryId == requestModel.CategoryId);

                if (category == null)
                {
                    return BadRequest("Category not found.");
                }

                _db.Foods.Add(new Food
                {
                    FoodName = requestModel.FoodName.Trim(),
                    CategoryId = requestModel.CategoryId,
                    Category = category.CategoryName,
                    Price = requestModel.Price,
                    IsAvailable = true
                });
                try
                {
                    int result = _db.SaveChanges();

                    string message = result > 0
                        ? "Food added successfully."
                        : "Failed to add food.";
                    return Ok(message);
                }
                catch (DbUpdateException ex)
                {
                    return BadRequest(ex.InnerException?.Message);
                }

            }
            [HttpPatch("{id}")]
            public IActionResult UpdateFood(int id, FoodUpdateRequestModel requestModel)
            {
                var food = _db.Foods.FirstOrDefault(x => x.FoodId == id);
                if (food is null)
                {
                    return NotFound("Food not found");
                }

                if (!string.IsNullOrWhiteSpace(requestModel.FoodName))
                {
                    food.FoodName = requestModel.FoodName.Trim();
                }

                if (requestModel.Price.HasValue)
                {
                    food.Price = requestModel.Price.Value;
                }
                if (requestModel.CategoryId.HasValue)
                {
                    food.CategoryId = requestModel.CategoryId.Value;
                }

                if (requestModel.IsAvailable.HasValue)
                {
                    food.IsAvailable = requestModel.IsAvailable.Value;
                }

                int result = _db.SaveChanges();
                string message = result > 0 ? "Food updated successfully" : "Failed to update food";

                return Ok(message);
            }

            [HttpDelete("{id}")]
            public IActionResult DeleteFood(int id)
            {
                var food = _db.Foods.FirstOrDefault(x => x.FoodId == id && x.IsAvailable == true);
                if (food is null)
                {
                    return NotFound("Food not found");
                }
                food.IsAvailable = false;

                int result = _db.SaveChanges();
                string message = result > 0 ? "Food deleted successfully" : "Failed to delete food";

                return Ok(message);
            }


        }
    }

