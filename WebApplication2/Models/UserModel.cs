using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Làm ơn nhập tên đăng nhập")]
        public string Username { get; set; }
        [DataType(DataType.Password),Required(ErrorMessage ="Làm ơn nhập mật khẩu")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Làm ơn nhập email"),EmailAddress]
        public string Email { get;set; }
    }
}
