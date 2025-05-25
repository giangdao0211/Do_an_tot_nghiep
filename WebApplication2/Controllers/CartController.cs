using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication2.Models;
using WebApplication2.Models.ViewModels;
using WebApplication2.Repository;

namespace WebApplication2.Controllers
{
    public class CartController : Controller
    {
        private readonly DataContext _dataContext;
        public CartController(DataContext _context)
        {
            _dataContext = _context;
        }
        public IActionResult Index(CouponModel coupon)
        {
            List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            var shippingPriceCookie = Request.Cookies["ShippingPrice"];
            decimal shippingPrice = 0;
            //decimal discountPrice = 0;

            if (shippingPriceCookie != null)
            {
                var shippingPriceJson = shippingPriceCookie;
                shippingPrice = JsonConvert.DeserializeObject<decimal>(shippingPriceJson);
            }
            //if (coupon.Status == 1)
            //{
            //    discountPrice = 1000;
            //}
            //else if(coupon.Status == 2)
            //{
            //    discountPrice = 2000;
            //}
                CartItemViewModel cartVM = new()
                {
                    CartItems = cartItems,
                    GrandTotal = cartItems.Sum(x => x.Quantity * x.Price + shippingPrice),
                    ShippingCost = shippingPrice,
                    //DiscountPrice = discountPrice
                };
            return View(cartVM);
        }
        public IActionResult Checkout()
        {
            return View("~/Views/Checkout/Index.cshtml");
        }
        public async Task<IActionResult> Add(int Id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(Id);
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemModel cartItems = cart.Where(c => c.ProductId == Id).FirstOrDefault();
            if (cartItems == null)
            {
                cart.Add(new CartItemModel(product));
            }
            else
            {
                cartItems.Quantity += 1;
            }
            HttpContext.Session.SetJson("Cart", cart);
            TempData["success"] = "Add item to cart successfully";
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public async Task<IActionResult> Decrease(int Id)
        {
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
            CartItemModel cartItems = cart.Where(c => c.ProductId == Id).FirstOrDefault();
            if (cartItems.Quantity > 1)
            {
                --cartItems.Quantity;
            }
            else
            {
                cart.RemoveAll(p => p.ProductId==Id);
            }
            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
            TempData["success"] = "Decrease item quantity successfully";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Increase(int Id)
        {
            ProductModel product= await _dataContext.Products.Where(p=>p.Id==Id).FirstOrDefaultAsync();
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
            CartItemModel cartItems = cart.Where(c => c.ProductId == Id).FirstOrDefault();
            if (cartItems.Quantity >= 1 && product.Quantity>cartItems.Quantity)
            {
                ++cartItems.Quantity;
            }
            else
            {
                cartItems.Quantity = product.Quantity;
                TempData["success"] = "Số lượng sản phẩm đã đạt tối đa";
                //cart.RemoveAll(p => p.ProductId == Id);
            }
            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
            TempData["success"] = "Increase item quantity successfully";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Remove(int Id)
        {
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
            cart.RemoveAll(p => p.ProductId == Id);
            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
            TempData["success"] = "Remove item  successfully";
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Route("Cart/GetShipping")]
        public async Task<IActionResult> GetShipping(ShippingModel shippingModel, string quan, string tinh, string phuong)
        {

            var existingShipping = await _dataContext.Shippings
                .FirstOrDefaultAsync(x => x.City == tinh && x.District == quan && x.Ward == phuong);

            decimal shippingPrice = 0; // Set mặc định giá tiền

            if (existingShipping != null)
            {
                shippingPrice = existingShipping.Price;
            }
            else
            {
                //Set mặc định giá tiền nếu ko tìm thấy
                shippingPrice = 50000;
            }
            var shippingPriceJson = JsonConvert.SerializeObject(shippingPrice);
            try
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(30),
                    Secure = true // using HTTPS
                };

                Response.Cookies.Append("ShippingPrice", shippingPriceJson, cookieOptions);
            }
            catch (Exception ex)
            {
                //
                Console.WriteLine($"Error adding shipping price cookie: {ex.Message}");
            }
            return Json(new { shippingPrice });
        }
    }
}
