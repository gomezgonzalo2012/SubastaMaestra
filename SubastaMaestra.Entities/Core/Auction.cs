using SubastaMaestra.Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SubastaMaestra.Entities.Core
{
    public class Auction //subasta
    {
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public AuctionState CurrentState { get; set; } // proxima/pendiente, activa, finalizada, cancelada
        [JsonIgnore] 
        public List<Product>? Products { get; set; } = new List<Product>();
    }
}