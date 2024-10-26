using SubastaMaestra.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Entities.Core
{
    public class Sale
    {
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; } //unique
        [Required]
        public int BuyerId { get; set; } // unique juntos
        public Product? Product { get; set; }
        public User? Buyer { get; set; } // comprador
        [Required]
        public float Amount { get; set; }
        [Required]
        public DateTime SaleDate { get; set; }

        public float? Deduccion { get; set; } 
        public PaymentMethods? PaymentMethod { get; set; }


    }
}
