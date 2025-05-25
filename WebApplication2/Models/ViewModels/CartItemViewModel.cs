namespace WebApplication2.Models.ViewModels
{
    public class CartItemViewModel
    {
        public List<CartItemModel> CartItems { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal ShippingCost { get; set; }
        //public decimal DiscountPrice { get; set; }
        public decimal TotalProduct
        {
            get { return GrandTotal - ShippingCost; }
        }
    }
}
