using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Models.DTOs.Reports
{
    public class AuctionReportDTO
    {
        public int AuctionId { get; set; }
        public string Title { get; set; }
        public DateTime FinishDate { get; set; }
        public int TotalOfertas { get; set; } // Cantidad total de ofertas en la subasta
        public decimal HighestBidAmount { get; set; }
    }




}
