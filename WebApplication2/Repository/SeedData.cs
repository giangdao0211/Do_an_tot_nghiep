using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Repository
{
    public class SeedData
    {
        public static void SeedingData(DataContext _context)
        {
            _context.Database.Migrate();
            if (!_context.Products.Any())
            {
                CategoryModel pc = new CategoryModel { Name = "Pc", Slug = "pc", Description = "Pc is the best", Status = 1 };
                CategoryModel laptop = new CategoryModel { Name = "Laptop", Slug = "laptop", Description = "Laptop is very portable ", Status = 1 };
                BrandModel apple = new BrandModel { Name = "Dell", Slug = "dell", Description = "Dell is the best", Status = 1 };
                BrandModel asus = new BrandModel { Name = "Asus", Slug = "asus", Description = "Asus is the best", Status = 1 };
                _context.Products.AddRange(
                    new ProductModel { Name = "PC HIỆU SUẤT GAMING CAO RTX 4060 - I5 12400F", Slug = "pc hiệu suất gaming cao rtx 4060 - i5 12400f", Description = "Full White", Image = "1.jpg", Category = pc, Brand = asus, Price = 15980, Ram = 16, Ssd = 512, Warranty = "24 tháng" },
                    new ProductModel { Name = "PC ASUS GAMING PERFORMANCE RTX 4060 - I5 12400F", Slug = "pc asus gaming performance rtx 4060 - i5 12400f", Description = "Mạnh", Image = "2.jpg", Category = pc, Brand = asus, Price = 15680, Ram = 16, Ssd = 512, Warranty = "36 tháng" },
                    new ProductModel { Name = "Laptop Apple Macbook Pro 14", Slug = "laptop apple macbook pro 14", Description = "Khoẻ", Image = "3.jpg", Category = laptop, Brand = apple, Price = 50000, Ram = 24, Ssd = 512, Warranty = "36 tháng" },
                    new ProductModel { Name = "Laptop Apple Macbook Air", Slug = "laptop apple macbook air", Description = "Khoẻ", Image = "4.jpg", Category = laptop, Brand = apple, Price = 26199, Ram = 24, Ssd = 512, Warranty = "36 tháng" }

                );
                _context.SaveChanges();
            }
        }
    }
}
