using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopRazor.Data;
using ShopRazor.models;

namespace ShopRazor.Pages.products
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public List<Poducts> ProductList { get; set; }
        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
            ProductList=_context.Products.ToList();
        }
    }
}
