using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication2.Models;
using WebApplication2.Repository;
using WebApplication2.Areas.Admin.Repository;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IEmailSender _emailSender;
        public CheckoutController(DataContext context, IEmailSender emailSender)
        {
            _dataContext = context;
            _emailSender = emailSender;
        }
        public async Task<IActionResult> Checkout()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return RedirectToAction("Login","Account");
            }
            else
            {
                var ordercode = Guid.NewGuid().ToString();
                var orderItem = new OrderModel();
                orderItem.OrderCode = ordercode;
                orderItem.UserName = userEmail;
                orderItem.Status = 1;
                orderItem.CreatedDate = DateTime.Now;
                _dataContext.Add(orderItem);
                _dataContext.SaveChanges();
                List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
                foreach(var cart in cartItems)
                {
                    var orderdetails = new OrderDetails();
                    orderdetails.UserName = userEmail;
                    orderdetails.OrderCode = ordercode;
                    orderdetails.ProductId = cart.ProductId;
                    orderdetails.Price = cart.Price;
                    orderdetails.Quantity = cart.Quantity;
                    //update product quantity
                    var product = await _dataContext.Products.Where(p=>p.Id==cart.ProductId).FirstAsync();
                    product.Quantity -= cart.Quantity;
                    product.Sold += cart.Quantity;
                    _dataContext.Update(product);
                    _dataContext.Add(orderdetails);
                    _dataContext.SaveChanges();
                }
                HttpContext.Session.Remove("Cart");
                //Send mail
                var receiver = userEmail;
                var subject = "Đặt hàng thành công.";
                var message = "Bạn đã đặt hàng thành công";
                await _emailSender.SendEmailAsync(receiver, subject, message);
                TempData["success"] = "Checkout thành công, vui lòng chờ duyệt đơn hàng";
                return RedirectToAction("History", "Account");
            }
            return View();
        }
        
    }
}
