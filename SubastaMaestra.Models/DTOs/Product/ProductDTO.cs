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
    public class ProductDTO
    {
        public string Name { get; set; }
        public ProductConditions? Condition { get; set; } // condicion
        public float InitialPrice { get; set; }
        public float? FinalPrice { get; set; }
        public string? ImgUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Description { get; set; }
        public int NumberOfOffers { get; set; }
        public int CategoryId { get; set; }
        public int SellerId { get; set; }
        public int BuyerId { get; set; }
        public int AuctionId { get; set; }
        public DeliveryModes? DeliveryCondition { get; set; }
    }
}
