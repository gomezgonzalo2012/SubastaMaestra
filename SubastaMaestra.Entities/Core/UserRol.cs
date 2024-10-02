using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubastaMaestra.Entities.Core
{
    public class UserRol
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
    }
}