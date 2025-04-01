using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RentnRoll.Data;
using Microsoft.AspNetCore.Http.HttpResults;

namespace RentnRoll.Pages
{
    public class BookingModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public BookingModel(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Booking booking { get; set; } = new();
        public Vehicle vehicle { get; set; }
        decimal tprice { get; set; }
        public List<BookingDatesDTO> BookedDates { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            vehicle = await _context.Vehicles.FindAsync(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            // Hämta bokade datum och lagra det i DTO:n
            BookedDates = await _context.Bookings
                .Where(b => b.VehicleID == id && b.Status == "Confirmed")
                .Select(b => new BookingDatesDTO
                {
                    PickupDate = b.PickupDate.ToString("yyyy-MM-dd"),
                    ReturnDate = b.ReturnDate.ToString("yyyy-MM-dd")
                })
                .ToListAsync();

            return Page();
        }



        public async Task<IActionResult> OnPostAsync(int id, DateTime PickupDate, DateTime ReturnDate)
        {
            vehicle = await _context.Vehicles.FindAsync(id);
            
            if (vehicle == null)
            {
                return NotFound();
            }

            var totalDays = (ReturnDate - PickupDate).Days;
            if (totalDays <= 0)
            {
                ModelState.AddModelError("", "Return date must be after pickup date.");
                return Page();
            }

            var totalCost = totalDays * vehicle.PricePerDay;
            var user = await _userManager.GetUserAsync(User);

            booking = new Booking
            {
                UserID = user.Id,
                User = user,
                VehicleID = vehicle.VehicleID,
                Vehicle = vehicle,
                PickupDate = PickupDate,
                ReturnDate = ReturnDate,
                TotalCost = totalCost,
                Status = "Pending",
                BookingDate = DateTime.Now
            };

            _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
			TempData["SuccessMessage"] = "Thank you for choosing Rent & Roll! Your booking has been registred.";
			return RedirectToPage("BookingSummary", new { id = booking.BookingID});
        }
    }
}
