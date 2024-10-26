using SubastaMaestra.Entities.Enums;
using SubastaMaestra.Entities.Core;
using SubastaMaestra.Models.DTOs.Product;
using SubastaMaestra.Models.DTOs.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Models.DTOs.Sale
{
    public class SaleDTO
    {
        public int Id { get; set; }
        public ProductDTO Product{ get; set; }
        public UserDTO? Buyer { get; set; } // comprador
        public UserDTO? Seller { get; set; } // vendedor
        public float Amount { get; set; }
        public DateTime SaleDate { get; set; }

        public float? Deduccion { get; set; }
        public PaymentMethods? PaymentMethod { get; set; }
    }
}
