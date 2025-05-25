using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Repository;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly DataContext _dataContext;
        public OrderController(DataContext context)
        {
            _dataContext = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Orders.OrderByDescending(p => p.Id).ToListAsync());
        }
        public async Task<IActionResult> ViewOrder(string ordercode)
        {
            var DetailsOrder = await _dataContext.Orders.Where(od => od.OrderCode == ordercode).ToListAsync();
            return View(DetailsOrder);
        }
        [HttpPost]
        [Route("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(string ordercode, int status)
        {
            var order = await _dataContext.Orders.FirstOrDefaultAsync(o => o.OrderCode == ordercode);
            if (order == null)
            {
                return NotFound();
            }
            order.Status = status;
            _dataContext.Update(order);
            if (status == 0)
            {
                var DetailsOrder = await _dataContext.OrderDetails.Where(od => od.OrderCode == order.OrderCode).Select(od => new
                {
                    od.Quantity,
                    od.Price,
                }).ToListAsync();
                var statisticalModel = await _dataContext.Statistical.FirstOrDefaultAsync(s => s.DateCreated.Date == order.CreatedDate.Date);
                if (statisticalModel != null)
                {
                    foreach (var orderDetail in DetailsOrder)
                    {
                        statisticalModel.Quantity += 1;
                        statisticalModel.Sold += orderDetail.Quantity;
                        statisticalModel.Revenue += (int)(orderDetail.Price * orderDetail.Quantity);
                    }
                    _dataContext.Update(statisticalModel);
                }
                else
                {
                    int new_quantity = 0;
                    int new_sold = 0;
                    decimal new_profit = 0;
                    foreach (var orderDetail in DetailsOrder)
                    {
                        new_quantity += 1;
                        new_sold += orderDetail.Quantity;
                        statisticalModel = new StatisticalModel
                        {
                            DateCreated = order.CreatedDate,
                            Quantity = new_quantity,
                            Sold = new_sold,
                            Revenue = (int)(orderDetail.Price * orderDetail.Quantity)
                        };
                    }
                    _dataContext.Add(statisticalModel);
                }
            }
            try
            {
                await _dataContext.SaveChangesAsync();
                return Ok(new { success = true, message = "Order status update successfully" });
            }catch(Exception ex)
            {
                return StatusCode(500, "An error orcurred while updating order status");
            }
        }
        [Route("Delete")]
        public async Task<IActionResult> Delete(int Id)
        {
            OrderModel order = await _dataContext.Orders.FindAsync(Id);

            _dataContext.Orders.Remove(order);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Xoá đơn hàng thành công";
            return RedirectToAction("Index");
        }
    }
}
