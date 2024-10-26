
using Microsoft.EntityFrameworkCore;
using SubastaMaestra.Data.SubastaMaestra.Data;
using SubastaMaestra.Entities.Core;
using SubastaMaestra.Entities.Enums;
using System.Text;

namespace SubastaMaestra.API.HostedServices
{
    public class AuctionBackgroundService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceProvider _serviceProvider;
        
        public AuctionBackgroundService(IServiceProvider serviceProvider, ILogger<AuctionBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Configurar el timer para que se ejecute cada X minutos (por ejemplo, cada minuto)
            //_timer = new Timer(ProcessAuction, null, TimeSpan.Zero, TimeSpan.FromMinutes(10));
            var tiempoRestante = CalcularTiempoHastaMedianoche();

            // Configurar el timer para que se ejecute cuando llegue la medianoche
            _timer = new Timer(ProcessAuction, null, tiempoRestante, TimeSpan.FromHours(24)); // Ejecutar cada 24 horas después de la primera ejecución
            return Task.CompletedTask;
        }
        
        private void ProcessAuction(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<SubastaContext>(); // Inyectar el DbContext

                ProcessPendingAuctions(dbContext);
                ProcessClosedAuctions(dbContext);
                
            }
        }

        private void ProcessClosedAuctions( SubastaContext localContext)
        {
            // Obtener todas las subastas que han llegado a su fecha de finalización y están activas
            var subastasFinalizadas = localContext.Auctions
                .Where(s => s.FinishDate <= DateTime.Now && s.CurrentState == AuctionState.Active)
                .ToList();

            if (subastasFinalizadas.Count == 0)
            {
                return;
            }

            foreach (var subasta in subastasFinalizadas)
            {
                subasta.CurrentState = AuctionState.Closed; // Cambiar el estado de la subasta a cerrada

                // Desactivar todos los productos de la subasta
                var productos = localContext.Products.Where(p => p.AuctionId == subasta.Id).ToList();
                foreach (var producto in productos)
                {
                    producto.CurrentState = ProductState.Disabled; // Cambiar el estado del producto a Desactivado

                    // Determinar el ganador si el producto tiene ofertas
                    var mejorOferta = localContext.Bids
                        .Where(o => o.ProductId == producto.Id)
                        .OrderByDescending(o => o.Price)
                        .FirstOrDefault();

                    if (mejorOferta != null)
                    {
                        producto.BuyerId = mejorOferta.BidderId; // Asignar el ganador del producto
                        producto.CurrentState = ProductState.Sold;
                    }
                }


            }
            // Guardar cambios en la base de datos
            localContext.SaveChanges();
        }
        private void ProcessPendingAuctions( SubastaContext localContext)
        {
            var pendingAuctions = localContext.Auctions
                .Where(s => s.StartDate <= DateTime.Now && s.CurrentState == AuctionState.Pending)
                .ToList();
            if (pendingAuctions.Count == 0)
            {
                return;
            }
            foreach (var auction in pendingAuctions)
            {
                //var hasProducts = localContext.Products.Any(p => p.AuctionId == auction.Id);
                var products = localContext.Products.Where(p => p.Id == auction.Id).ToList();
                if (products.Count>0) // activar si tiene productos
                {
                    auction.CurrentState = AuctionState.Active;
                    // activar productos
                    foreach (var p in products)
                    {
                        p.CurrentState = ProductState.Active;
                    }
                }else
                {
                    auction.CurrentState = AuctionState.Canceled;  // nuevo estado "WaitingForProduct"
                }
            }
            // Guardar cambios en la base de datos
            localContext.SaveChanges();
        }
        
        private TimeSpan CalcularTiempoHastaMedianoche()
        {
            // Calcular el tiempo restante hasta la próxima medianoche
            var ahora = DateTime.Now;
            var siguienteMedianoche = DateTime.Today.AddDays(1).Date;
            return siguienteMedianoche - ahora;
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
