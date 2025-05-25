using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Repository;

namespace WebApplication2.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<AppUserModel> _userManager;
        public ProductController(DataContext context, UserManager<AppUserModel> userManager)
        {
            _dataContext = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var products = _dataContext.Products.ToList();
            return View(products);
        }
        public async Task<IActionResult> Details(int Id)
        {
            if (Id == null)
            {
                return RedirectToAction("Index");
            }
            var productsById = _dataContext.Products.Where(p => p.Id == Id).FirstOrDefault();
            //related products
            var relatedProducts = await _dataContext.Products
                .Where(p => p.CategoryId == productsById.CategoryId && p.Id != productsById.Id).Take(4)
                .ToListAsync();
            ViewBag.RelatedProducts = relatedProducts;
            return View(productsById);
        }
        public async Task<IActionResult> Search(string searchTerm)
        {
            var products = await _dataContext.Products.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm)).ToListAsync();
            ViewBag.Keyword = searchTerm;
            return View(products);
        }
        public async Task<IActionResult> AddWishlist(int Id)
        {
            var user=await _userManager.GetUserAsync(User);
            var wishlistProduct = new WishlistModel
            {
                ProductId = Id,
                UserId = user.Id
            };
            _dataContext.Wishlists.Add(wishlistProduct);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Add item to wishlist successfully";
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public async Task<IActionResult> Wishlist()
        {
           var wishlist_product= await (from w in _dataContext.Wishlists
                                        join p in _dataContext.Products on w.ProductId equals p.Id
                                        select new {Product=p,Wishlists=w}).ToListAsync();
            return View(wishlist_product);
        }
        public async Task<IActionResult> DeleteWishlist(int Id)
        {
            WishlistModel wishlist = await _dataContext.Wishlists.FindAsync(Id);

            _dataContext.Wishlists.Remove(wishlist);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Xoá sản phẩm yêu thích thành công";
            return RedirectToAction("Wishlist","Product");
        }
    }
}
