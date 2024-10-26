using SubastaMaestra.Entities.Core;
using SubastaMaestra.Entities.Enums;
using SubastaMaestra.Models.DTOs.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SubastaMaestra.Models.DTOs.Auction
{
    public class AuctionDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public AuctionState State { get; set; } // 0 inactivo, 1 habilitado, 2 deshabilitado

        
        public List<ProductDTO>? Products { get; set; } = new List<ProductDTO>();
    }
}
