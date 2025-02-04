using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopRazor.Data;
using ShopRazor.models;

namespace ShopRazor.Pages.products
{
    [BindProperties]
    public class CreateModel : PageModel
    {
      
        private readonly ApplicationDbContext _context;
        [BindProperty]
        public Poducts Poducts { get; set; }
        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
        }
        public IActionResult OnPost(Poducts obj)
        {
            _context.Products.Add(Poducts);
            _context.SaveChanges();
            return RedirectToPage("Index");
        }
        public IActionResult test(Poducts obj)
        {
            _context.Products.Add(obj);
            _context.SaveChanges();
            return RedirectToPage("Index");
        }
    }
}
