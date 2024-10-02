using SubastaMaestra.Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SubastaMaestra.Entities.Core
{
    public class User
    {
        
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public DocumType DocumentType { get; set; }
        [Required]
        public string DocumentNumber { get; set; }
        public string? PhoneNumber { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        public State State { get; set; }
        public UserRol Rol { get; set; }
        public int RolId { get; set; }
        [Required]
        public PersonType? PersonType { get; set; } // Juridica o Fisica




    }
}