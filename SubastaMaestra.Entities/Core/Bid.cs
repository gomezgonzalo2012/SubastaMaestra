using SubastaMaestra.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Entities.Core
{

    public class Bid // pujan :  usuarios_productos
    { 
        public int ProductId { get; set; }
        public int BidderId { get; set; }
        public Product Product{ get; set; }
        public User Bidder { get; set; } // pujador
        [Required]
        public float Price { get; set; }
        public DateTime OfferDate { get; set; } // fecha de puja
        [Required]
        public PaymentMethods PaymentMethods { get; set; }
    }
}
