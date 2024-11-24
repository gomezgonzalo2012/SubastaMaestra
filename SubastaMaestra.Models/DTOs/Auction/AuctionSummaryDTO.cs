using SubastaMaestra.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Models.DTOs.Auction
{
    public class AuctionSummaryDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public AuctionState CurrentState { get; set; } // 0 inactivo, 1 habilitado, 2 deshabilitado

    }

}
