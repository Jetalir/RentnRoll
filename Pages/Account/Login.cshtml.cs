using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;


namespace RentnRoll.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _singInManager;

        public LoginModel(SignInManager<IdentityUser> singInManager)
        {
            _singInManager = singInManager;
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

            var result = await _singInManager.PasswordSignInAsync(Email, Password, false, false);

            if (result.Succeeded) 
            {
                return RedirectToPage("/Index");
            }

            ErrorMessage = "Inloggningen misslyckades. Vänligen försök igen!";
            return Page();

        }

    }
}
