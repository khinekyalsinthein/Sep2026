using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sep2026.ConsoleApp4.Database.AppDbContextModels;
using Sep2026.RestaurantOrderingWebApi.Models;

namespace Sep2026.RestaurantOrderingWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
            private readonly AppDbContext _db;

            public CategoriesController()
            {
                _db = new AppDbContext();
            }

            [HttpGet]
            public IActionResult GetCategories()
            {
                var categories = _db.Categories.ToList();
                return Ok(categories);
            }

            [HttpGet("{id}")]
            public IActionResult GetCategory(int id)
            {
                var category = _db.Categories.FirstOrDefault(x => x.CategoryId == id);
                if (category is null)
                {
                    return NotFound("Category not found");
                }
                return Ok(category);
            }

            [HttpPost]
            public IActionResult AddCategory(CategoryCreateRequestModel requestModel)
            {
                if (string.IsNullOrWhiteSpace(requestModel.CategoryName))
                {
                    return BadRequest("Category name is required");
                }
                bool isDuplicate = _db.Categories.Any(c => c.CategoryName.ToLower() == requestModel.CategoryName.Trim().ToLower());
                if (isDuplicate)
                {
                    return BadRequest("Category name already exists");
                }
                _db.Categories.Add(new Category
                {
                    CategoryName = requestModel.CategoryName.Trim()
                });
                int result = _db.SaveChanges();
                string message = result > 0 ? "Category added successfully" : "Failed to add category";
                return Ok(message);
            }

            [HttpPut("{id}")]
            public IActionResult UpdateCategory(int id, CategoryUpsertRequestModel requestModel)
            {
                var category = _db.Categories.FirstOrDefault(x => x.CategoryId == id);
                if (category is null)
                {
                    return NotFound("Category not found");
                }
                if (string.IsNullOrWhiteSpace(requestModel.CategoryName))
                {
                    return BadRequest("Category name is required");
                }
                bool exists = _db.Categories.Any(x => x.CategoryName.ToLower() == requestModel.CategoryName.Trim().ToLower() && x.CategoryId != id);
                if (exists)
                {
                    return BadRequest("Category name already exists");
                }
                category.CategoryName = requestModel.CategoryName.Trim();
                int result = _db.SaveChanges();
                string message = result > 0 ? "Category updated successfully" : "Failed to update category";
                return Ok(message);
            }

            [HttpDelete("{id}")]
            public IActionResult DeleteCategory(int id)
            {
                var category = _db.Categories.FirstOrDefault(x => x.CategoryId == id);
                if (category is null)
                {
                    return NotFound("Category not found");
                }
                bool hasFoods = _db.Foods.Any(x => x.CategoryId == id);
                if (hasFoods)
                {
                    return BadRequest("Cannot delete category because foods are using it.");
                }
                _db.Categories.Remove(category);
                int result = _db.SaveChanges();
                string message = result > 0 ? "Category deleted successfully" : "Failed to delete category";
                return Ok(message);
            }

            [HttpGet("{id}/foods")]
            public IActionResult GetFoodsByCategoryId(int id)
            {
                var category = _db.Categories.Find(id);

                if (category == null)
                {
                    return NotFound("Category not found.");
                }

                var foods = _db.Foods
                    .Where(f => f.CategoryId == id)
                    .Select(f => new
                    {
                        f.FoodId,
                        f.FoodName,
                        f.Price,
                        f.CategoryId,
                        f.IsAvailable
                    })
                    .ToList();

                return Ok(foods);
            }

        }
    }

