using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.CompilerServices;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[Secured]
public class AdminController : Controller
{
    private readonly IWebHostEnvironment _hostingEnvironment;

    // GET
    public AdminController(IWebHostEnvironment env)
    {
        _hostingEnvironment = env;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Boots()
    {
        ProductRepository pr = new ProductRepository();

        this.ViewBag.Boots = pr.FindAll();

        return View();

    }

    public IActionResult Order()
    {  
        MyContext db = new MyContext();
        this.ViewBag.Orders = db.TbOrders.Include(x=>x.Customer).ToList();
        this.ViewBag.OrderDetails = db.TbOrderDetails.Include(x=>x.Variation.Color).Include(X=>X.Variation.Boot.TbPhotos).Include(X=>X.Variation.Boot).Include(x=>x.Variation).ToList();
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    public IActionResult LogOut()
    {
        this.HttpContext.Session.Remove("login");
        return RedirectToAction("Index", "Home");
    }

    public IActionResult EditBoot(int id)
    {
        ProductRepository pr = new ProductRepository();
        BootModel boot = new BootModel(pr.FindById(id));
        return View(boot);
    }

    [HttpPost]
    public IActionResult EditBootPost(BootModel bootPost)
    {
        MyContext mc = new MyContext();

        TbBoot tb = mc.TbBoots.First(x => x.BootId == bootPost.BootId);
        for (var i = 0; i < bootPost.Variations.Count; i++)
        {
            TbVariation tv = bootPost.Variations[i];
            tv.Color = bootPost.Colors.Find(x=>x.ColorId == tv.ColorId);
        }

        tb.TbVariations = bootPost.Variations;
        tb.TbPhotos = bootPost.Photos;
        tb.ShortDescription= bootPost.ShortDescription;
        tb.LongDescription = bootPost.LongDescription;
        tb.Material = bootPost.Material;
        tb.Code = bootPost.Code;
        tb.Name = bootPost.Name;
        foreach (var category in mc.TbBootCategories)
        {
            if(category.BootId == bootPost.BootId)
                mc.TbBootCategories.Remove(category);
        }
        
        foreach (TbCategory tc in bootPost.Categories)
        {
            TbBootCategory tbc = new TbBootCategory();
            tbc.BootId = bootPost.BootId;
            tbc.CategoryId = tc.CategoryId;
            mc.TbBootCategories.Add(tbc);
        }
        mc.SaveChanges();
        

        return View("EditBoot", bootPost);
    }

    public IActionResult EditBootAddCategory(BootModel bootPost, int id)
    {
        TbCategory tc = bootPost.NonCategories.First(x => x.CategoryId == id);
        bootPost.Categories.Add(tc);
        bootPost.NonCategories.Remove(tc);
        MyContext mc = new MyContext();
        mc.TbBootCategories.Add(new TbBootCategory() {BootId = bootPost.BootId, CategoryId = id});
        mc.SaveChanges();
        
        return View("EditBoot", bootPost);
    }

    public IActionResult EditBootDeleteCategory(BootModel bootPost, int id)
    {
        TbCategory tc = bootPost.Categories.First(x => x.CategoryId == id);
        bootPost.NonCategories.Add(tc);
        bootPost.Categories.Remove(tc);
        MyContext mc = new MyContext();
        mc.TbBootCategories.Remove(mc.TbBootCategories.First(x => x.BootId == bootPost.BootId && x.CategoryId == id));
        mc.SaveChanges();
        
        return View("EditBoot", bootPost);
    }

    public IActionResult EditBootAddPhoto(BootModel bootPost)
    {
        if (bootPost.Image != null)
        {
            MyContext mc = new MyContext();
            int pathID = mc.TbPhotos.Max(x => x.PhotoId) + 1;
            string wwwpath = _hostingEnvironment.WebRootPath;
            string finalPath = Path.Combine(wwwpath, "BootPhotos", pathID + Path.GetExtension(bootPost.Image.FileName));
            using FileStream fs = new(finalPath, FileMode.Create);
            bootPost.Image.CopyTo(fs);

            TbPhoto tbPhoto = new TbPhoto();
            string dbPath = Path.Combine("BootPhotos", pathID + Path.GetExtension(bootPost.Image.FileName));
            tbPhoto.Path = dbPath;
            tbPhoto.BootId = bootPost.BootId;
            mc.TbPhotos.Add(tbPhoto);
            mc.SaveChanges();
            bootPost.Photos.Add(tbPhoto);
            bootPost.Image = null;
        }

        return View("EditBoot", bootPost);
    }

    public IActionResult EditBootDeletePhoto(BootModel bootPost, int id)
    {
        TbPhoto photo = bootPost.Photos.First(x => x.PhotoId == id);
        bootPost.Photos.Remove(photo);
        string wwwpath = _hostingEnvironment.WebRootPath;
        MyContext mc = new MyContext();
        mc.TbPhotos.Remove(mc.TbPhotos.First(x => x.PhotoId == id));
        mc.SaveChanges();
        string finalPath = Path.Combine(wwwpath, photo.Path);

        System.IO.File.Delete(finalPath);
        return View("EditBoot", bootPost);
    }

    public IActionResult EditBootAddColor(BootModel bootPost)
    {
        MyContext mc = new MyContext();
        TbColor color = new TbColor();
        color.Color = "Nová barva";
        color.Hexadecimal = "#000000";
        mc.TbColors.Add(color);
        mc.SaveChanges();

        TbVariation variation = new TbVariation();
        variation.BootId = bootPost.BootId;
        variation.ColorId = color.ColorId;
        variation.Discount = 0;
        variation.Dph = 20;
        variation.Size = 40;
        variation.InStock = 0;
        variation.Price = 500;
        mc.TbVariations.Add(variation);
        mc.SaveChanges();

        bootPost.Variations.Add(variation);
        bootPost.Colors.Add(color);
        return View("EditBoot", bootPost);
    }

    public IActionResult EditBootChooseColor(BootModel bootPost, int id)
    {
        bootPost.PickedVariationId = -1;
        bootPost.PickedColorId = id;
        return View("EditBoot", bootPost);
    }

    public IActionResult EditBootDeleteColor(BootModel bootPost, int id)
    {
        MyContext mc = new MyContext();
        List<TbVariation> tbVariations =
            mc.TbVariations.Where(x => x.BootId == bootPost.BootId && x.ColorId == id).ToList();
        bootPost.Variations.Clear();

        foreach (TbVariation variation in tbVariations)
        {
            mc.TbVariations.Remove(variation);
        }

        mc.TbColors.Remove(mc.TbColors.First(x => x.ColorId == id));
        mc.SaveChanges();
        bootPost.PickedColorId = -1;
        bootPost.PickedVariationId = -1;
        bootPost.Colors.Remove(bootPost.Colors.First(x => x.ColorId == id));

        return View("EditBoot", bootPost);
    }

    public IActionResult EditBootAddSize(BootModel bootPost)
    {
     
        MyContext mc = new MyContext();


        TbVariation variation = new TbVariation();
        variation.BootId = bootPost.BootId;
        variation.ColorId = bootPost.PickedColorId;
        variation.Discount = 0;
        variation.Dph = 20;
        if(bootPost.Variations.Where(x=>x.ColorId == bootPost.PickedColorId).Count() == 0)
            variation.Size = 40;
        else
            variation.Size = mc.TbVariations.Where(x => x.BootId == bootPost.BootId && x.ColorId == bootPost.PickedColorId).Max(x => x.Size) + 1;
        variation.InStock = 1;
        variation.Price = 500;
        mc.TbVariations.Add(variation);
        mc.SaveChanges();
        bootPost.Variations.Add(variation);
        return View("EditBoot", bootPost);
    }

    public IActionResult EditBootChooseSize(BootModel bootPost, int size)
    {
        MyContext mc = new MyContext();
        
        TbVariation variation = bootPost.Variations.First(x => x.BootId == bootPost.BootId
                                                           &&
                                                           x.Size == size
                                                           &&
                                                           x.ColorId == bootPost.PickedColorId);
        bootPost.PickedVariationId = variation.VariationId;
        
        return View("EditBoot", bootPost);
    }
    public IActionResult EditBootDeleteSize(BootModel bootPost, decimal size)
    {
        MyContext mc = new MyContext();

        mc.TbVariations.Remove( mc.TbVariations.First(x => x.BootId == bootPost.BootId
                                                           &&
                                                           x.Size == size
                                                           &&
                                                           x.ColorId == bootPost.PickedColorId));
        bootPost.Variations.Remove(bootPost.Variations.First(x => x.BootId == bootPost.BootId
                                                           &&
                                                           x.Size == size
                                                           &&
                                                           x.ColorId == bootPost.PickedColorId));
         mc.SaveChanges();
        
        bootPost.PickedVariationId = -1;
         return View("EditBoot", bootPost);
    }

}
