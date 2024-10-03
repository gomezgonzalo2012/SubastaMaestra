
using Microsoft.EntityFrameworkCore;
using SubastaMaestra.Data.SubastaMaestra.Data;
using SubastaMaestra.Entities.Enums;
using System.Text;

namespace SubastaMaestra.API.HostedServices
{
    public class AuctionBackgroundService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceProvider _serviceProvider;
        private readonly string logFilePath = "C:\\Users\\gomez\\source\\repos\\Metodologia"; // Ruta del archivo en el servidor
        private readonly ILogger<AuctionBackgroundService> _logger;
        public AuctionBackgroundService(IServiceProvider serviceProvider, ILogger<AuctionBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Configurar el timer para que se ejecute cada X minutos (por ejemplo, cada minuto)
            _timer = new Timer(ProcessAuction, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
            return Task.CompletedTask;
        }
        
        private void ProcessAuction(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<SubastaContext>(); // Inyectar el DbContext

                // Obtener todas las subastas que han llegado a su fecha de finalización y están activas
                var subastasFinalizadas = dbContext.Auctions
                    .Where(s => s.FinishDate <= DateTime.Now && s.CurrentState == AuctionState.Active)
                    .ToList();

                if(subastasFinalizadas.Count == 0)
                {
                    return;
                }

                foreach (var subasta in subastasFinalizadas)
                {
                    subasta.CurrentState = AuctionState.Closed; // Cambiar el estado de la subasta a Desactivado

                    // Desactivar todos los productos de la subasta
                    var productos = dbContext.Products.Where(p => p.AuctionId == subasta.Id).ToList();
                    foreach (var producto in productos)
                    {
                        producto.CurrentState = ProductState.Disabled; // Cambiar el estado del producto a Desactivado

                        // Determinar el ganador si el producto tiene ofertas
                        var mejorOferta = dbContext.Bids
                            .Where(o => o.ProductId == producto.Id)
                            .OrderByDescending(o => o.Price)
                            .FirstOrDefault();

                        if (mejorOferta != null)
                        {
                            producto.BuyerId = mejorOferta.BidderId; // Asignar el ganador del producto
                        }
                    }
                      
                  
                }

                // Guardar cambios en la base de datos
                dbContext.SaveChanges();
                EscribirLog("Operacion realizada");
            }
        }
        private void EscribirLog(string mensaje)
        {
            try
            {
                // Asegurarse de que el directorio exista
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp");
                var directoryPath = Path.GetDirectoryName(path);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Agregar la línea al final del archivo de log
                using (var streamWriter = new StreamWriter(logFilePath, append: true, encoding: Encoding.UTF8))
                {
                    streamWriter.WriteLine(mensaje);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al escribir en el archivo de log: {ex.Message}");
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
