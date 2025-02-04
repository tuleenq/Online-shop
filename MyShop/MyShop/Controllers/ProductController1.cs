using Microsoft.AspNetCore.Mvc;

namespace MyShop.Controllers
{
    public class ProductController1 : Controller
    {
        public IActionResult products()
        {
            
            return View();
        }
    }
}
