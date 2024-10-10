using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VeriMedCNS.Models;
using System.Linq;
using VeriMedCNS.Data;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using System;

namespace VeriMedCNS.Pages.Users
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

        public IActionResult OnPost()
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

            bool a = BCrypt.Net.BCrypt.Verify(Password, user.PasswordHash);
            if (user != null && a)
            {
                // Restablecer intentos al tener �xito
                loginAttempts = 0;
                return RedirectToPage("/Patients/Index"); // Usuario encontrado y contrase�a correcta
            }
            else
            {
                // Si el usuario no es encontrado o la contrase�a es incorrecta
                loginAttempts++;

                if (loginAttempts >= maxLoginAttempts)
                {
                    // Bloquear los intentos por 5 minutos
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
