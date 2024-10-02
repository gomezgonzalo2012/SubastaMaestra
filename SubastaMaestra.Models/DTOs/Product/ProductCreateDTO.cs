using SubastaMaestra.Entities.Core;
using SubastaMaestra.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Models.DTOs.Product
{
    public class ProductCreateDTO
    {
        [Required]
        [StringLength(maximumLength: 50)]
        public string Name { get; set; }
        public ProductConditions? Condition { get; set; } // condicion
        [Required]
        public float InitialPrice { get; set; }

        public string? ImgUrl { get; set; }

        [Required]
        [StringLength(maximumLength: 800)]
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public int SellerId { get; set; }
        public int AuctionId { get; set; }
        public DeliveryModes? DeliveryCondition { get; set; }
        //public PaymentMethods? PaymentMethods { get; set; }
    }
}
