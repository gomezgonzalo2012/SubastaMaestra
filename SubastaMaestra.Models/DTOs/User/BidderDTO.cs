using SubastaMaestra.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Models.DTOs.User
{
    public class BidderDTO
    {
        public int BidderId { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public DateTime OfferDate { get; set; }
        public PaymentMethods PaymentMethod { get; set; }
    }
}
