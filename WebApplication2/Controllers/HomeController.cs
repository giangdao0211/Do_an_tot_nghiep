using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Repository;

namespace WebApplication2.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DataContext _dataContext;

    public HomeController(ILogger<HomeController> logger, DataContext context)
    {
        _logger = logger;
        _dataContext = context;
    }

    public IActionResult Index()
    {
        var products = _dataContext.Products.ToList();
        return View(products);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(int statuscode)
    {
        if (statuscode == 404)
        {
            return View("NotFound");
        }
        else
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
