using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class LoginController : Controller
{
    
    private Authenticator auth = new Authenticator();
    
    // GET
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(TbAdmin model)
    {
        if (this.auth.Login(model.Login, model.Password))
        {
            this.HttpContext.Session.SetString("login", model.Login);
            return RedirectToAction("Index", "Admin");
        }
        
        return View(model);
    }

}