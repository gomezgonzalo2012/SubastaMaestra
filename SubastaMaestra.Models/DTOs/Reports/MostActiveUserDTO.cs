using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Models.DTOs.Reports
{
    public class MostActiveUserDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int TotalBids  { get; set; }
        public float HighestBidAmount { get; set; }
        public string ProductName { get; set; }
        public DateTime HighestBidDate { get; set; }
    }

}
