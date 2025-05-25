using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication2.Repository.Validation;

namespace WebApplication2.Models
{
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }
        [Required( ErrorMessage = "Yêu cầu nhập tên sản phẩm")]
        public string Name { get; set; }
        public string Slug { get; set; }
        [Required, MinLength(1, ErrorMessage = "Yêu cầu nhập mô tả sản phẩm")]
        public string Description { get; set; }
        [Required ( ErrorMessage = "Yêu cầu nhập giá sản phẩm")]
        [Range(0.01,double.MaxValue)]
        [Column(TypeName ="decimal(18,2)")]
        public decimal Price { get; set; }
        public string Warranty { get; set; }
        public int Ram { get; set; }
        public int Ssd { get; set; }
        public int Quantity { get; set; }
        public int Sold { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public CategoryModel Category { get; set; }
        public BrandModel Brand { get; set; }
        public string Image { get; set; } 
        [NotMapped]
        [FileExtension]
        public IFormFile? ImageUpload { get; set; }
    }
}
