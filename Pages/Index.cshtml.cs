using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RentnRoll.Data;

namespace RentnRoll.Pages
{
    public class IndexModel : PageModel
    {
		private readonly AppDbContext _context;
		public IndexModel(AppDbContext context)
        {
			_context = context;
		}

		public List<Vehicle> Vehicle { get; set; }

        public List<string> VehicleBrands { get; set; } = new(); // List of available vehicle brands
        public List<string> VehicleTypes { get; set; } = new(); // List of available vehicle types


        //Sorting and filtering
        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; } = "price"; // Default sort by price
        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; } = "asc"; // Default Ascending order
        [BindProperty(SupportsGet = true)]
        public string FilterBrand { get; set; } = "";  // Default no filter
        [BindProperty(SupportsGet = true)]
        public string FilterType { get; set; } = ""; // Default no filter

        public Dictionary<int, DateTime> NextAvailableDate { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            VehicleBrands = await _context.Vehicles.Select(v => v.Brand).Distinct().ToListAsync();
            VehicleTypes = await _context.Vehicles.Select(v => v.Type).Distinct().ToListAsync();

            IQueryable<Vehicle> vehicles = _context.Vehicles;

            if (!string.IsNullOrEmpty(FilterBrand))
            {
                vehicles = vehicles.Where(v => v.Brand == FilterBrand);
            }

            if (!string.IsNullOrEmpty(FilterType))
            {
                vehicles = vehicles.Where(v => v.Type == FilterType);
            }

            vehicles = SortBy switch
            {
                "brand" => SortOrder == "asc" ? vehicles.OrderBy(v => v.Brand) : vehicles.OrderByDescending(v => v.Brand),
                "year" => SortOrder == "asc" ? vehicles.OrderBy(v => v.Year) : vehicles.OrderByDescending(v => v.Year),
                _ => SortOrder == "asc" ? vehicles.OrderBy(v => v.PricePerDay) : vehicles.OrderByDescending(v => v.PricePerDay),
            };

            Vehicle = await vehicles.ToListAsync();

            // Hämta bokningar för alla bilar
            var bookings = await _context.Bookings
                .Where(b => b.Status == "Confirmed")
                .OrderBy(b => b.PickupDate)
                .ToListAsync();

            foreach (var vehicle in Vehicle)
            {
                var vehicleBookings = bookings
                    .Where(b => b.VehicleID == vehicle.VehicleID)
                    .ToList();

                if (vehicleBookings.Any())
                {
                    var currentDate = DateTime.Now.Date;

                    // Hitta lediga perioder innan nästa bokning
                    var firstBooking = vehicleBookings.First();
                    if (currentDate < firstBooking.PickupDate)
                    {
                        NextAvailableDate[vehicle.VehicleID] = currentDate; // Ledig före första bokningen
                        continue;
                    }

                    // Om bilen är mitt i en bokning just nu
                    var activeBooking = vehicleBookings
                        .FirstOrDefault(b => currentDate >= b.PickupDate && currentDate <= b.ReturnDate);

                    if (activeBooking != null)
                    {
                        NextAvailableDate[vehicle.VehicleID] = activeBooking.ReturnDate.AddDays(1); // Tillgänglig dagen efter bokningen
                    }
                    else
                    {
                        // Bilen är ledig mellan bokningar
                        var nextBooking = vehicleBookings
                            .FirstOrDefault(b => b.PickupDate > currentDate);

                        if (nextBooking != null)
                        {
                            NextAvailableDate[vehicle.VehicleID] = nextBooking.PickupDate.AddDays(-1); // Ledig innan nästa bokning
                        }
                        else
                        {
                            NextAvailableDate[vehicle.VehicleID] = currentDate; // Helt ledig
                        }
                    }
                }
                else
                {
                    NextAvailableDate[vehicle.VehicleID] = DateTime.Now; // Ingen bokning alls, ledig nu
                }
            }

            return Page();
        }

        public void OnPost()
        {
  
        }
    }
}
