using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using System.Linq;

namespace WebApplication1.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        ProductRepository pr = new ProductRepository();
        Random rnd = new Random();
        this.ViewBag.Boots = pr.FindAll().OrderBy(x => rnd.Next()).Take(8);
        return View();
    }


}