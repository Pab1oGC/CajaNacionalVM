using CNSVM.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace CNSVM.Pages.Users
{
    public class LoginModel : PageModel
    {
        private readonly CnsvmDbContext _context;
        private readonly IConfiguration _configuration;

        public LoginModel(CnsvmDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [BindProperty]
        
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        private static int loginAttempts = 0;
        private static DateTime blockUntil = DateTime.MinValue;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            int maxLoginAttempts = _configuration.GetValue<int>("LoginSettings:MaxLoginAttempts");
            int blockDuration = _configuration.GetValue<int>("LoginSettings:BlockDurationInMinutes");

            // Si el usuario est� bloqueado, no permitir m�s intentos
            if (blockUntil > DateTime.Now)
            {
                ErrorMessage = "Too many failed login attempts. Please try again later.";
                return Page();
            }

            // Buscar al usuario por el Username
            var user = _context.User.SingleOrDefault(u => u.Username == Username);

            if (user != null && BCrypt.Net.BCrypt.Verify(Password, user.PasswordHash))
            {
                // Restablecer intentos al tener �xito
                loginAttempts = 0;

                // Crear los claims del usuario
                var claims = new List<Claim>
                {
                     new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                };
                Console.WriteLine("Doctor ID (UserId): " + user.Id);
                // Crear la identidad y los claims
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // Establecer la autenticaci�n por cookies
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // Guardar el nombre de usuario y la contrase�a en cookies (si es necesario)
                Response.Cookies.Append("Username", Username);
                Response.Cookies.Append("Password", Password);

                return RedirectToPage("/Patients/Index");  // Usuario encontrado y contrase�a correcta
            }
            else
            {
                // Si el usuario no es encontrado o la contrase�a es incorrecta
                loginAttempts++;

                if (loginAttempts >= maxLoginAttempts)
                {
                    blockUntil = DateTime.Now.AddMinutes(blockDuration);
                    ErrorMessage = $"Too many failed attempts. Please try again in {blockDuration} minutes.";
                }
                else
                {
                    ErrorMessage = "Usuario o contrase�a incorrectos. Por favor, intente de nuevo.";
                }

                return Page();
            }
        }
    }
}
