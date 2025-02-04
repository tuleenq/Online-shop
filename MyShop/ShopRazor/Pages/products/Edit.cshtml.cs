using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopRazor.Data;
using ShopRazor.models;

namespace ShopRazor.Pages.products
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public Poducts Poducts { get; set; }
        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public void OnGet(int? id)
        {
            if(id != null&& id!=0)
            {
                Poducts = _context.Products.Find(id);
            }
        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _context.Products.Update(Poducts);
                _context.SaveChanges();
                //TempData["success"] = "product updated succefully";

                return RedirectToPage("Index");
            }
            return Page();
           
        }
    }
 }

