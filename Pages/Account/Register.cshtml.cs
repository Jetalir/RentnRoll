using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RentnRoll.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public RegisterModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = new IdentityUser
            {
                UserName = Email,
                Email = Email
            };

            var result = await _userManager.CreateAsync(user, Password);

            if (result.Succeeded)
            {
                // Nya anv�ndaren f�r rollen "User"
                await _userManager.AddToRoleAsync(user, "User");
                TempData["SuccessMessage"] = "Your account has been registered! You can now log in.";
                return RedirectToPage("/Account/Login");
            }

            ErrorMessage = "Registration failed. Please try again!";
            return Page();
        }
    }
}
