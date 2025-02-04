using Microsoft.AspNetCore.Mvc;

namespace ATS_CV.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
