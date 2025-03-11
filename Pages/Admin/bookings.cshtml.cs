using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentnRoll.Data;

namespace RentnRoll.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class bookingsModel : PageModel
    {
        private readonly AppDbContext _context;
        public List<Booking> Booking { get; set; }

        public bookingsModel(AppDbContext context)
        {
            _context = context;
            Booking = _context.Bookings.ToList();
            foreach (var booking in Booking)
            {
                booking.Vehicle = _context.Vehicles.FirstOrDefault(v => v.VehicleID == booking.VehicleID);
                booking.User = _context.Users.FirstOrDefault(u => u.Id == booking.UserID);
            }

        }
        
        public void OnGet()
        {

        }
        public void OnPost()
        {
            foreach (var booking in _context.Bookings.ToList())
            {
                string formKey = $"status_{booking.BookingID}";

                if (Request.Form.ContainsKey(formKey))
                {
                    string newStatus = Request.Form[formKey];

                    // Kolla om statusen har ändrats
                    if (!string.IsNullOrEmpty(newStatus) && newStatus != booking.Status)
                    {
                        booking.Status = newStatus;
                        _context.Update(booking);
                    }
                }
            }

            _context.SaveChanges();
        }
    }
}
