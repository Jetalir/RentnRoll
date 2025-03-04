using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RentnRoll.Data;

namespace RentnRoll.Pages.Admin
{
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;

        public DeleteModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Vehicle Vehicle { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Vehicle = await _context.Vehicles.FindAsync(id);

            if (Vehicle == null)
            {
                return NotFound(); // Om bilen inte finns, visa 404-sida
            }

            return Page();
        }
        
		public async Task<IActionResult> OnPostAsync(int id)
		{
            Vehicle = await _context.Vehicles.FindAsync(id);
            if (Vehicle == null)
			{
				return Page();
			}

            _context.Vehicles.Remove(Vehicle);
			await _context.SaveChangesAsync();
			return RedirectToPage("/Index");
		}

	}
}
