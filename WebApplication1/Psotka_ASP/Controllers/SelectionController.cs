using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class SelectionController : Controller
{
    // GET
    public IActionResult Index(string cat)
    {
        ProductRepository pr = new ProductRepository();
        MyContext context = new();
        if (cat == "f")
        {
            List<TbBoot> tbs = new();
            var cats = context.TbBootCategories.Where(x => x.CategoryId == 13);
            foreach (var item in cats)
            {
                tbs.Add(pr.FindById(item.BootId));
            }
            this.ViewBag.Boots = tbs;
        }
        else if (cat == "m")
        {
            List<TbBoot> tbs = new();
            var cats = context.TbBootCategories.Where(x => x.CategoryId == 21);
            foreach (var item in cats)
            {
                tbs.Add(pr.FindById(item.BootId));
            }
            this.ViewBag.Boots = tbs;
        }
        else
        {
            this.ViewBag.Boots = pr.FindAll();
        }


        return View();
    }
}