using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
		public void OnGet()
        {
            Vehicle = _context.Vehicles.ToList();
		}

        public void OnPost()
        {
  
        }
    }
}
