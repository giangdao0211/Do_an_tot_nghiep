﻿using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
