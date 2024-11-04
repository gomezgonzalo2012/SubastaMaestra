using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SubastaMaestra.Data.Interfaces;
using SubastaMaestra.Data;
using SubastaMaestra.Entities.Core;
using SubastaMaestra.Models.DTOs;
using SubastaMaestra.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using SubastaMaestra.Models.DTOs.User;
using SubastaMaestra.Models.DTOs.Product;

namespace SubastaMaestra.Data.Implements
{
    public class NotificacionRepository : INotificationRepository
    {
        private readonly SubastaContext _subastaContext;
        private readonly IMapper _mapper;
        public NotificacionRepository(SubastaContext subastaContext, IMapper mapper) {
            _subastaContext = subastaContext;
            _mapper = mapper;
        }
        public async Task<OperationResult<bool>> CreateNotification(int userId, int productId, NotificationType notificationType)
        {
            NotificationDTO notification = new NotificationDTO();
            notification.ProductoId = productId;
            notification.UserId = userId;
            notification.NotificationType = notificationType;
            notification.Created_at = DateTime.Now;
            notification.State = true;
            var product = _subastaContext.Products.Where(p=>p.Id == productId).FirstOrDefault();
            switch (notificationType) // se rellena contenido texual
            {
                case NotificationType.AcceptedNotification:
                    notification.Title = "Tu solicitud de producto fue aceptado";
                    notification.Body = $"Tu producto {product.Name} fué aceptado por los adminsitradores lo verás disponible para ver recibir ofertas al abrir la subasta.";
                break;
                case NotificationType.RejectedNotification:
                    notification.Title = "Tu solicitud de producto rechazada";
                    notification.Body = $"Tu producto {product.Name} no cumple los criterios de aceptación para publicar un producto." +
                        $" Asegurate de relenar todos los campos e intentalo denuevo";
                    break;
                case NotificationType.SellerNotification:
                    notification.Title = "Tú producto fue vendido";
                    notification.Body = $"Felicidades tu producto {product.Name} fué vendido a la ofeta máxima {product.FinalPrice}.";
                break;
                case NotificationType.BuyerNotification:
                    notification.Title = "Tú oferta fue la ganadora.";
                    notification.Body = $"Felicidades tu oferta por el producto {product.Name} ha sido la ganadora.";
                    break;

                default:
                    break;
            }

            try
            {
                var notif = _mapper.Map<Notification>(notification);
                await _subastaContext.Notifications.AddAsync(notif);
                await _subastaContext.SaveChangesAsync();
                return new OperationResult<bool> { Success = true };
            }
            catch (Exception ex)
            {
                return new OperationResult<bool> { Success = false, Message = $"Error al crear la notificacion {ex.Message}" };
            }

        }
        
        public async Task<OperationResult<List<NotificationDTO>>> GetAllNotificationsByUserAsync(int userId)
        {
            try
            {
                var notifListDTO = new List<NotificationDTO>();
                var notifList = await _subastaContext.Notifications.Where(n=>n.UserId==userId).OrderByDescending(n=>n.Created_at)
                    .Include(n=>n.Product)
                    .Include(n=>n.User).ToListAsync();
                if (notifList.Any())
                {
                    foreach (var notif in notifList)
                    {
                        var notifDTO = _mapper.Map<NotificationDTO>(notif);
                        notifListDTO.Add(notifDTO);
                    }
                    return new OperationResult<List<NotificationDTO>> { Success = true, Value = notifListDTO };

                }
                return new OperationResult<List<NotificationDTO>> { Success = true, Message="No hay notificaciones", Value = notifListDTO };


            }
            catch (Exception ex)
            {
                return new OperationResult<List<NotificationDTO>> { Success = false, Message = $"Error al traer las notificaciones {ex.Message}" };
            }
        }

        public async Task<OperationResult<NotificationDTO>> GetNotificationById(int notifId)
        {
            try
            {
                var notif = await _subastaContext.Notifications.Where(u=> u.Id== notifId).FirstOrDefaultAsync();
                if (notif != null)
                {
                    var notifDTO = new NotificationDTO();
                    notifDTO = _mapper.Map<NotificationDTO>(notif);
                    return new OperationResult<NotificationDTO> { Success = true, Value = notifDTO };

                }
                return new OperationResult<NotificationDTO> { Success = false, Message="No se pudo encontrar la notificacion" };
            }
            catch (Exception ex)
            {
                return new OperationResult<NotificationDTO> { Success = false, Message = $"Error al traer las notificaciones {ex.Message}" };
            }

        }
    }

        
    
}
