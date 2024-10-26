using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Models.DTOs.User
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage ="Correo requerido")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "El correo no puede ser menor a 10 caracteres")]
        public string Email { get; set; }
        [Required (ErrorMessage ="Contraseña requerido")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "La contraseña no puede ser menor a 8 caracteres")]
        public string Password { get; set; }
    }
}
