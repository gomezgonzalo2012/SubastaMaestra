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
        [Required(ErrorMessage ="El campo título es requerido.")]
        public string? Title { get; set; }
        //public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "El campo fecha de fín es requerido.")]
        public DateTime FinishDate { get; set; }
        [Required(ErrorMessage = "El campo fecha de inicio es requerido.")]
        public DateTime StartDate { get; set; }


    }
}
