using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.ViewModel;

namespace WebApplication1.Controllers;

public class CartController : Controller
{
    // GET
    public IActionResult Acknowledgement()
    {
        return View();
    }
    public IActionResult Index()
    {
        CustomerModel cm = new CustomerModel();
        this.ViewBag.Cart = new List<string>();
        this.ViewBag.Subtotal = 0;
        if (this.HttpContext.Session.GetString("cart") != null)
        {


            var Items =
                (JsonSerializer.Deserialize<List<int>>(this.HttpContext.Session.GetString("cart")) ?? new List<int>())
                .GroupBy(x => x).Select(x => new { Id = x.Key, Count = x.Count() }).ToList();
            MyContext db = new MyContext();
            Dictionary<TbVariation, int> cart = new Dictionary<TbVariation, int>();
            foreach (var v in Items)
            {
                cart.Add(db.TbVariations.Include(x=> x.Color).Include(x => x.Boot.TbPhotos).First(x => x.VariationId == v.Id), v.Count);
            }

            this.ViewBag.Cart = cart;
            foreach (var item in this.ViewBag.Cart)
            {
                int k = (int)item.Key.Price * item.Value;
                this.ViewBag.Subtotal += k;
            }
        }

        return View(cm);
    }

    public IActionResult AddToCart(CustomerModel cm, int id)
    {
        this.ViewBag.Cart = new List<string>();
        this.ViewBag.Subtotal = 0;
        MyContext db = new MyContext();
        if (this.HttpContext.Session.GetString("cart") != null)
        {
            List<int> cart1 = new List<int>();
            if (HttpContext.Session.GetString("cart") != null)
            {
                cart1 = JsonSerializer.Deserialize<List<int>>(HttpContext.Session.GetString("cart"));
            }

            if (cart1.Where(x => x == id).Count() < db.TbVariations.First(x => x.VariationId == id).InStock)
            {
                cart1.Add(id);
            }

            this.HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cart1));

            var Items =
                (JsonSerializer.Deserialize<List<int>>(this.HttpContext.Session.GetString("cart")) ?? new List<int>())
                .GroupBy(x => x).Select(x => new { Id = x.Key, Count = x.Count() }).ToList();
            Dictionary<TbVariation, int> cart = new Dictionary<TbVariation, int>();
            foreach (var v in Items)
            {
                cart.Add(db.TbVariations.Include(x=> x.Color).Include(x => x.Boot.TbPhotos).First(x => x.VariationId == v.Id), v.Count);
            }

            this.ViewBag.Cart = cart;
            foreach (var item in this.ViewBag.Cart)
            {
                int k = (int)item.Key.Price * item.Value;
                this.ViewBag.Subtotal += k;
            }
        }
        return View("Index", cm);
    }

    public IActionResult TakewayFromCart(CustomerModel cm, int id)
    {
        this.ViewBag.Cart = new List<string>();
        this.ViewBag.Subtotal = 0;
        if (this.HttpContext.Session.GetString("cart") != null)
        {
            List<int> cart1 = new List<int>();
            if (HttpContext.Session.GetString("cart") != null)
            {
                cart1 = JsonSerializer.Deserialize<List<int>>(HttpContext.Session.GetString("cart"));
            }

            if (cart1.Where(x => x == id).Count() > 1)
            {
                cart1.Remove(id);
            }

            this.HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cart1));

            var Items =
                (JsonSerializer.Deserialize<List<int>>(this.HttpContext.Session.GetString("cart")) ?? new List<int>())
                .GroupBy(x => x).Select(x => new { Id = x.Key, Count = x.Count() }).ToList();
            MyContext db = new MyContext();
            Dictionary<TbVariation, int> cart = new Dictionary<TbVariation, int>();
            foreach (var v in Items)
            {
                cart.Add(db.TbVariations.Include(x=> x.Color).Include(x => x.Boot.TbPhotos).First(x => x.VariationId == v.Id), v.Count);
            }

            this.ViewBag.Cart = cart;
            foreach (var item in this.ViewBag.Cart)
            {

                int k = (int)item.Key.Price * item.Value;
                this.ViewBag.Subtotal += k;
            }
        }
        return View("Index", cm);
    }

    public IActionResult DeleteToCart(CustomerModel cm, int id)
    {
        this.ViewBag.Cart = new List<string>();
        this.ViewBag.Subtotal = 0;
        if (this.HttpContext.Session.GetString("cart") != null)
        {
            List<int> cart1 = new List<int>();
            if (HttpContext.Session.GetString("cart") != null)
            {
                cart1 = JsonSerializer.Deserialize<List<int>>(HttpContext.Session.GetString("cart"));
            }

            cart1.RemoveAll(x => x == id);
            this.HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cart1));

            var Items =
                (JsonSerializer.Deserialize<List<int>>(this.HttpContext.Session.GetString("cart")) ?? new List<int>())
                .GroupBy(x => x).Select(x => new { Id = x.Key, Count = x.Count() }).ToList();
            MyContext db = new MyContext();
            Dictionary<TbVariation, int> cart = new Dictionary<TbVariation, int>();
            foreach (var v in Items)
            {
                cart.Add(db.TbVariations.Include(x=> x.Color).Include(x => x.Boot.TbPhotos).First(x => x.VariationId == v.Id), v.Count);
            }

            this.ViewBag.Cart = cart;
            foreach (var item in this.ViewBag.Cart)
            {

                int k = (int)item.Key.Price * item.Value;
                this.ViewBag.Subtotal += k;
            }
        }
         return View("Index", cm);
    }

    public IActionResult MakeOrder(CustomerModel cm)
    {

        MyContext db = new MyContext();
        TbCustomer tc = new TbCustomer();

        int subtotal = 0;

        if (this.HttpContext.Session.GetString("cart") != null)
        {


            var Items =
                (JsonSerializer.Deserialize<List<int>>(this.HttpContext.Session.GetString("cart")) ?? new List<int>())
                .GroupBy(x => x).Select(x => new { Id = x.Key, Count = x.Count() }).ToList();
            Dictionary<TbVariation, int> cart = new Dictionary<TbVariation, int>();
            foreach (var v in Items)
            {
                cart.Add(db.TbVariations.Include(x=> x.Color).Include(x => x.Boot.TbPhotos).First(x => x.VariationId == v.Id), v.Count);
            }
            if(cart.Count == 0)
            {
                this.ViewBag.Cart = new List<string>();
                this.ViewBag.Subtotal = 0;
                return View("Index", cm);
            }
            foreach (var item in cart)
            {
                int k = (int)item.Key.Price * item.Value;
                subtotal += k;
                db.TbVariations.First(x => x.VariationId == item.Key.VariationId).InStock -= item.Value;
                db.SaveChanges();
            }
        }
        else
        {
            this.ViewBag.Cart = new List<string>();
            this.ViewBag.Subtotal = 0;
            return View("Index", cm);
        }

        tc.Adress = cm.Address;
        tc.Email = cm.Email;
        tc.Name = cm.Name;
        tc.Phone = cm.Phone;
        tc.Country = cm.Country;
        tc.PostalCode = cm.PostalCode;
        tc.Surname = cm.Surname;
        db.TbCustomers.Add(tc);

        db.SaveChanges();

        TbOrder to = new TbOrder();
        to.CustomerId = tc.CustomerId;
        to.Date = DateTime.Now;
        to.Discount = cm.Discount;
        to.Price = subtotal;
        to.Dph = 21;
        db.TbOrders.Add(to);

        db.SaveChanges();

        if (this.HttpContext.Session.GetString("cart") != null)
        {


            var Items =
                (JsonSerializer.Deserialize<List<int>>(this.HttpContext.Session.GetString("cart")) ?? new List<int>())
                .GroupBy(x => x).Select(x => new { Id = x.Key, Count = x.Count() }).ToList();
            Dictionary<TbVariation, int> cart = new Dictionary<TbVariation, int>();
            foreach (var v in Items)
            {
                cart.Add(db.TbVariations.Include(x=>x.Color).Include(x => x.Boot.TbPhotos).First(x => x.VariationId == v.Id), v.Count);
            }
            this.ViewBag.Cart = cart;
            foreach (var item in this.ViewBag.Cart)
            {
                TbOrderDetail tod = new TbOrderDetail();
                tod.OrderId = to.OrderId;
                tod.Color = item.Key.Color.Color;
                tod.Size = item.Key.Size;
                tod.Name = item.Key.Boot.Name;
                tod.Photo = item.Key.Boot.TbPhotos[0].Path;
                tod.VariationId = item.Key.VariationId;
                tod.Price = item.Key.Price;
                tod.Discount = item.Key.Discount;
                tod.Dph = item.Key.Dph;
                tod.Quantity = item.Value;
                db.TbOrderDetails.Add(tod);

                db.SaveChanges();
            }
        }

        this.HttpContext.Session.Remove("cart");

        return View("Acknowledgement");
    }

}