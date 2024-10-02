using SubastaMaestra.Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SubastaMaestra.Entities.Core
{
    
    public class Product
    {
       
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public ProductConditions? Condition { get; set; } // condicion
        [Required]
        public float InitialPrice { get; set; } 
        public float? FinalPrice { get; set; } 
        public string? ImgUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        public string? Description { get; set; }
        public ProductState CurrentState { get; set; }
        public int NumberOfOffers { get; set; }
        public ProductCategory Category { get; set; }
        public int? CategoryId { get; set; }
        public User Seller { get; set; }
        public int SellerId { get; set; }
        public User Buyer { get; set; } // comprador puede ser nulo
        public int? BuyerId { get; set; } 
        public Auction Auction { get; set; }
        public int AuctionId { get; set; } 
        public DeliveryModes? DeliveryCondition { get; set; }


        // agregar metodo de pago

    }
}
