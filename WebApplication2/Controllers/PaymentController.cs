﻿using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Services.Momo;

namespace WebApplication2.Controllers
{
    public class PaymentController : Controller
    {
        private IMomoService _momoService;
        public PaymentController(IMomoService momoService)
        {
            _momoService = momoService;
        }
        [HttpPost]
        public async Task<IActionResult> CreatePaymentMomo(OrderInfo model)
        {
            var response = await _momoService.CreatePaymentMomo(model);
            return Redirect(response.PayUrl);
        }
        [HttpGet]
        public IActionResult PaymentCallBack()
        {
            var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
            return View(response);
        }
    }
}
