using SubastaMaestra.Entities.Enums;
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
        [Range(0,float.MaxValue)]
        public float Amount { get; set; }// precio
        [Required(ErrorMessage = "El campo producto id es requerido")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "El campo id usuario es requerido")]
        public int BidderId { get; set; }
        [Required(ErrorMessage = "El campo metodo de pago usuario es requerido")]
        public PaymentMethods PaymentMethods { get; set; }
    }
}
