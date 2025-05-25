using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.ViewModels
{
    public class LoginViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Làm ơn nhập tên đăng nhập")]
        public string Username { get; set; }
        [DataType(DataType.Password), Required(ErrorMessage = "Làm ơn nhập mật khẩu")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
