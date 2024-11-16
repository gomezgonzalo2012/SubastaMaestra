using SubastaMaestra.Entities.Enums;
using SubastaMaestra.Models.DTOs.Product;
using SubastaMaestra.Models.DTOs.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Models.DTOs.Bid
{
    public class BidDTO
    {
        public int ProductId { get; set; }
        public int BidderId { get; set; }
        public ProductDTO Product { get; set; }
        public UserDTO Bidder { get; set; } // pujador
        public float Price { get; set; }
        public DateTime OfferDate { get; set; } // fecha de puja
        public PaymentMethods PaymentMethods { get; set; }
    }
}
