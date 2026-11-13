namespace Sep2026.RestaurantOrderingWebApi.Models
{
    public class OrderCreateRequestModel
    {
        public List<AddToCartModel> Items { get; set; }
        = new List<AddToCartModel>();
    }

    public class OrderUpdateRequestModel
    {
        public int? FoodId { get; set; }
        public int? Quantity { get; set; }

    }
}
