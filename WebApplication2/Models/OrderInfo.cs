using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class OrderInfo
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string OrderId { get; set; }
        public string OrderInformation { get; set; }
        public double Amount { get; set; }

    }
}
