namespace Sep2026.RestaurantOrderingWebApi.Models
{
    public class CartItemModel
    {
        public int FoodId { get; set; }
        public string FoodName { get; set; } = "";
        public int Quantity { get; set; }
        public decimal FoodPrice { get; set; }
        public decimal Total { get; set; }

    }
}
