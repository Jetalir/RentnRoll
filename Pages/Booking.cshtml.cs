using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using RentnRoll.Data;

namespace RentnRoll.Pages
{
    public class BookingModel : PageModel
    {
		private readonly AppDbContext _context;

		public BookingModel(AppDbContext context)
		{
			_context = context;
		}

		public List<Vehicle> Cart { get; set; } = new();
		public Vehicle Vehicle { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
		{
			Vehicle = await _context.Vehicles.FindAsync(id);
			if (Vehicle == null)
			{
                return Page();
            }
			else
			{
				Cart.Add(Vehicle);
			}
			return Page();
		}
    }
}
