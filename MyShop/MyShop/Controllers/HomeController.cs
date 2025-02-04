using Microsoft.AspNetCore.Mvc;

namespace MyShop.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(string category)
        {
            return View(model: category);
        }
        public IActionResult Products(string category)
        {
            ViewBag.Category = category; // Pass the category to the view
            return View();
        }
        public IActionResult Product(string name)
        {
            ViewBag.ProductName = name; // Pass the product name to the view
            return View();
        }
        public IActionResult Payment(string name)
        {
            ViewBag.ProductName = name; // Pass the product name to the view
            return View();
        }

        public IActionResult Search(string name)
        {
            ViewBag.ProductName = name; // Pass the product name to the view
            return View();
        }


        public IActionResult SearchList(string name)
        {
            ViewBag.ProductName = name; // Pass the product name to the view
            return View();
        }


        public IActionResult SearchBrand(string name)
        {
            ViewBag.ProductName = name; // Pass the product name to the view
            return View();
        }

        public IActionResult Guide(string name)
        {
            ViewBag.ProductName = name; // Pass the product name to the view
            return View();
        }

        public IActionResult electricguid(string name)
        {
            ViewBag.ProductName = name; // Pass the product name to the view
            return View();
        }

        public IActionResult proelectric(string name)
        {
            ViewBag.ProductName = name; // Pass the product name to the view
            return View();
        }

        public IActionResult acousticpeg(string name)
        {
            ViewBag.ProductName = name; // Pass the product name to the view
            return View();
        }

        public IActionResult acousticpro(string name)
        {
            ViewBag.ProductName = name; // Pass the product name to the view
            return View();
        }

        public IActionResult bassbeg(string name)
        {
            ViewBag.ProductName = name; // Pass the product name to the view
            return View();
        }

        public IActionResult basspro(string name)
        {
            ViewBag.ProductName = name; // Pass the product name to the view
            return View();
        }

        public IActionResult About(string name)
        {
            ViewBag.ProductName = name; // Pass the product name to the view
            return View();
        }

        public IActionResult Contact(string name)
        {
            ViewBag.ProductName = name; // Pass the product name to the view
            return View();
        }
        
        public IActionResult Privacy(string name)
        {
            ViewBag.ProductName = name; // Pass the product name to the view
            return View();
        }

        public IActionResult handbeg(string name)
        {
            ViewBag.ProductName = name; // Pass the product name to the view
            return View();
        }
        public IActionResult handpro(string name)
        {
            ViewBag.ProductName = name; // Pass the product name to the view
            return View();
        }
        public IActionResult delebeg(string name)
        {
            ViewBag.ProductName = name; // Pass the product name to the view
            return View();
        }
        public IActionResult delepro(string name)
        {
            ViewBag.ProductName = name; // Pass the product name to the view
            return View();
        }
        public IActionResult dacosticbeg(string name)
        {
            ViewBag.ProductName = name; // Pass the product name to the view
            return View();
        }
        public IActionResult dacousticpro(string name)
        {
            ViewBag.ProductName = name; // Pass the product name to the view
            return View();
        }
        
        public IActionResult brassbeg(string name)
        {
            ViewBag.ProductName = name; // Pass the product name to the view
            return View();
        }

        public IActionResult brasspro(string name)
        {
            ViewBag.ProductName = name; // Pass the product name to the view
            return View();
        }

        public IActionResult stringsbeg(string name)
        {
            ViewBag.ProductName = name; // Pass the product name to the view
            return View();
        }
        public IActionResult stringspro(string name)
        {
            ViewBag.ProductName = name; // Pass the product name to the view
            return View();
        }

    }
}
