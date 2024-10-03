using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Models.DTOs.Bid
{
    public class BidCreateDTO
    {
        [Required(ErrorMessage ="El campo monto es requerido")]
        public float Amount; // precio
        [Required(ErrorMessage = "El campo producto id es requerido")]
        public int ProductId;
        [Required(ErrorMessage = "El campo id usuario es requerido")]
        public int UserId;

    }
}
