using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class CouponModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Yêu cầu nhập tên coupon")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập mô tả coupon")]
        public string Description { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateExpired { get; set; }
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập số lượng coupon")]
        public int Quantity { get; set; }
        public int Status { get; set; } 
    }
}
