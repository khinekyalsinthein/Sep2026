namespace Sep2026.RestaurantOrderingWebApi.Models
{
    public class ReceiptModel
    {
        public int OrderId { get; set; }

        public List<CartItemModel> Items { get; set; }
            = new List<CartItemModel>();

        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }

        public string Message { get; set; } = "";
    }
    public class CheckoutRequestModel
    {
        public int OrderId { get; set; }
    }
}

