using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CNSVM.Pages.Users
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnPostAsync()
        {
            // Eliminar la autenticación
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Eliminar cookies
            Response.Cookies.Delete("Username");
            Response.Cookies.Delete("Password");

            return RedirectToPage("Login");
        }
    }
}