using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RentnRoll.Data;

namespace RentnRoll.Pages
{
    public class BookingHistoryModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public List<Booking> Bookings { get; set; } = new();

        public BookingHistoryModel(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User); // Hämtar den inloggade användaren

            if (user != null)
            {
                Bookings = await _context.Bookings
                    .Where(b => b.UserID == user.Id)
                    .Include(b => b.Vehicle) // Laddar in fordonet direkt
                    .ToListAsync();
            }
        }
    }
}
