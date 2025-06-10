using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class OrderDetails
    {
        public int Id { get;set; }
        public string UserName { get; set; }
        public string UserPhone
        {
            get { return "0962139566"; }
        }
        public string OrderCode { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get;set; }
        public int Quantity { get; set; }
        public decimal GrandTotal
        {
            get { return Quantity*Price+50000; }
        }
        //[ForeignKey("ProductId")]
        //public ProductModel Product { get; set; }
    }
}
