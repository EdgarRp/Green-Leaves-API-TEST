using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GreenLeaves.Data.ViewModels {
    public class LoginViewModel {

        [Required(ErrorMessage = "El nombre de usuario o e-mail es requerido")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "La contraseña no puede ser nula")]
        public string Password { get; set; }
    }

    public class UserViewModel {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string UserName { get; set; }
        [Required]
        public string Rol { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
    public class UserMailViewModel {
        [Required(ErrorMessage = "Nombre requerido")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Teléfono requerido")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "E-mail requerido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Ciudad es requerida")]
        public string City { get; set; }
        [Required(ErrorMessage = "Fecha es requerida")]
        public DateTime Date { get; set; }
    }
}
