using System;

namespace VeriMedCNS.Models
{
    public class User
    {

        public int UserId { get; set; } // Clave primaria
        public string Username { get; set; } // Nombre de usuario
        public string PasswordHash { get; set; } // Hash de la contraseña
        public string Role { get; set; } // Rol del usuario (por ejemplo, doctor, director)
        public DateTime CreatedAt { get; set; } // Fecha de creación
        public byte Status { get; set; } // Estado del usuario (tinyint)
        public string Name { get; set; } // Nombre
        public string FirstName { get; set; } // Primer nombre
        public string LastName { get; set; } // Apellido
        public string Email { get; set; } // Correo electrónico
    }
}
