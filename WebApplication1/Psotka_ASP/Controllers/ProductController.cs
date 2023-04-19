using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using System.Linq;
using System.Text.Json;

namespace WebApplication1.Controllers;

public class ProductController : Controller
{
    // GET
    public IActionResult Index(int id)
    {
        ProductRepository pr = new ProductRepository();
        BootModel boot = new BootModel(pr.FindById(id));
        Random rnd = new Random();
        this.ViewBag.Boots = pr.FindAll().OrderBy(x => rnd.Next()).Take(4);
        boot.PickedColorId = boot.Variations.First().ColorId;
        boot.PickedVariationId = boot.Variations.First().VariationId;

        return View(boot);
    }

    public IActionResult ProductChooseColor(BootModel bootPost, int id)
    {
        ProductRepository pr = new ProductRepository();
        Random rnd = new Random();
        this.ViewBag.Boots = pr.FindAll().OrderBy(x => rnd.Next()).Take(4);
        bootPost.PickedColorId = id;
        bootPost.PickedVariationId = bootPost.Variations.Find(x => x.ColorId == id).VariationId;
        return View("Index", bootPost);
    }
    public IActionResult ProductChooseSize(BootModel bootPost, int id)
    {
        ProductRepository pr = new ProductRepository();
        Random rnd = new Random();
        this.ViewBag.Boots = pr.FindAll().OrderBy(x => rnd.Next()).Take(4);
        bootPost.PickedVariationId = id;
        return View("Index", bootPost);
    }
    public IActionResult ProductAddToCart(BootModel bootPost, int id)
    {
        ProductRepository pr = new ProductRepository();
        Random rnd = new Random();
        MyContext db = new MyContext();
        this.ViewBag.Boots = pr.FindAll().OrderBy(x => rnd.Next()).Take(4);
        List<int> cart = new List<int>();
        if (HttpContext.Session.GetString("cart") != null)
        {
            cart = JsonSerializer.Deserialize<List<int>>(HttpContext.Session.GetString("cart"));
        }
        if (cart.Where(x => x == id).Count() < db.TbVariations.First(x => x.VariationId == id).InStock)
        {
            cart.Add(id);
        }
        this.HttpContext.Session.SetString("cart",JsonSerializer.Serialize(cart));
        return View("Index", bootPost);
    }
    


}