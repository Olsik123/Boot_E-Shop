using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Components;

public class NavComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
    
}