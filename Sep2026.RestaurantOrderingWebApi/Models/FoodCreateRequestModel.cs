namespace Sep2026.RestaurantOrderingWebApi.Models
{
    public class FoodCreateRequestModel
    {
        public string FoodName { get; set; } = "";
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }
    public class FoodUpdateRequestModel
    {
        public string? FoodName { get; set; }
        public decimal? Price { get; set; }
        public int? CategoryId { get; set; }
        public bool? IsAvailable { get; set; }
    }

}

