using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Models.DTOs.Reports
{
    public class ProfitReportDTO
    {
        public int AuctionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string AuctionTitle { get; set; }
        public int TotalProductosVendidos { get; set; }
        public float MontoTotalVentas { get; set; }
        public float GananciaTotal { get; set; }
    }

}
