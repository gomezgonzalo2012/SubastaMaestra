using SubastaMaestra.Models.DTOs.Product;
using SubastaMaestra.Models.DTOs.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Models.DTOs
{
    public class NotificationDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public DateTime Created_at { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ProductoId { get; set; }
        public bool? State { get; set; }
        public ProductDTO? Product { get; set; }
        public UserDTO? User { get; set; }

        public NotificationType NotificationType { get; set; }
    }
    public enum NotificationType
    {
        SellerNotification, // producto vendido
        AcceptedNotification, // producto aceptado
        BuyerNotification, // producto comprado
        RejectedNotification // producto rechazado 

    }


}

