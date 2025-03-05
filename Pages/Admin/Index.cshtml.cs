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
        public CreateVehicleDTO createVehicleDTO { get; set; }
        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Vehicle vehicle = new Vehicle
            {
                Type = createVehicleDTO.Type,
                Brand = createVehicleDTO.Brand,
                Model = createVehicleDTO.Model,
                Year = createVehicleDTO.Year,
                PricePerDay = createVehicleDTO.PricePerDay,
                TransmissionType = createVehicleDTO.TransmissionType,
                Description = createVehicleDTO.Description,
                ImageURL = createVehicleDTO.ImageURL
            };

            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }

    public class CreateVehicleDTO
    {
        public string Type { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal PricePerDay { get; set; }
        public string TransmissionType { get; set; }
        public string? Description { get; set; }
        public string? ImageURL { get; set; }

    }
}
