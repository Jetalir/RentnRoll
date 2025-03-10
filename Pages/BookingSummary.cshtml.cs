using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RentnRoll.Data;

namespace RentnRoll.Pages
{
    public class BookingSummaryModel : PageModel
    {

        private readonly AppDbContext _context;
		public BookingSummaryModel(AppDbContext context)
        {
            _context = context;
        }

        public Booking booking { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            booking = await _context.Bookings.Include(b => b.Vehicle).FirstOrDefaultAsync(b => b.BookingID == id);

			if (booking == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
