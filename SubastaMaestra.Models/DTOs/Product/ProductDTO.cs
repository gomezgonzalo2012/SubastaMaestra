using SubastaMaestra.Entities.Core;
using SubastaMaestra.Entities.Enums;
using SubastaMaestra.Models.DTOs.Auction;
using SubastaMaestra.Models.DTOs.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SubastaMaestra.Models.DTOs.Product
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ProductConditions? Condition { get; set; } // condicion
        public float InitialPrice { get; set; }
        public float? FinalPrice { get; set; } = 0;
        public string? ImgUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Description { get; set; }
        public ProductState CurrentState { get; set; }
        public int NumberOfOffers { get; set; }
        public int CategoryId { get; set; }
        public int SellerId { get; set; }
        public String? SellerName { get; set; }
        public int BuyerId { get; set; }
        public String? BuyerName { get; set; }
        public int AuctionId { get; set; }
        //[JsonIgnore]
        //public AuctionDTO? Auction { get; set; }
        public AuctionSummaryDTO? Auction { get; set; }
        public DeliveryModes? DeliveryCondition { get; set; }
    }
}
