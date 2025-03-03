using RentnRoll.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RentnRoll.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        [BindProperty]
        public Vehicle Vehicle { get; set; }

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Vehicles.Add(Vehicle);
            _context.SaveChanges();

            return RedirectToPage("Index");
        }
    }
}
