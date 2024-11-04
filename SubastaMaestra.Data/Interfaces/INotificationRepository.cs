using SubastaMaestra.Models.DTOs;
using SubastaMaestra.Models.DTOs.Product;
using SubastaMaestra.Models.DTOs.User;
using SubastaMaestra.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Data.Interfaces
{
    public interface INotificationRepository
    {
        Task<OperationResult<bool>> CreateNotification(int userId, int productId, NotificationType notificationType);
        Task<OperationResult<List<NotificationDTO>>> GetAllNotificationsByUserAsync(int userId);
        Task<OperationResult<NotificationDTO>> GetNotificationById(int notifId);
    }
}
