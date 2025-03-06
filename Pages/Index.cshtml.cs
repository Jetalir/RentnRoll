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
            return Page();
		}

        public void OnPost()
        {
  
        }
    }
}
