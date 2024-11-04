using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Entities.Core
{
    public class Notification
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public DateTime Created_at{ get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ProductoId { get; set; }
        public bool? State { get; set; }
        public Product? Product { get; set; }
        public User? User { get; set; }

    }
}
