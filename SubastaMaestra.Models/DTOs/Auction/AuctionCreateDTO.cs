using SubastaMaestra.Entities.Core;
using SubastaMaestra.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SubastaMaestra.Models.DTOs.Auction
{
    public class AuctionCreateDTO
    {

        public string? Title { get; set; }
        //public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }


    }
}
