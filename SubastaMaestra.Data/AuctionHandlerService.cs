using Microsoft.EntityFrameworkCore;
using SubastaMaestra.Data.SubastaMaestra.Data;
using SubastaMaestra.Entities.Core;
using SubastaMaestra.Entities.Enums;

namespace SubastaMaestra.Data
{
    public  class AuctionHandlerService
    {
        private  readonly SubastaContext _dbContext;
        public AuctionHandlerService( SubastaContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task ProcessAuctions()
        {
            {

                await ProcessPendingAuctions();
                await ProcessClosedAuctions();

            }
        }
        public async Task ProcessClosedAuctions( )
        {
            // Obtener todas las subastas que han llegado a su fecha de finalización y están activas
            var subastasFinalizadas = await _dbContext.Auctions
                .Where(s => s.FinishDate <= DateTime.Now && s.CurrentState == AuctionState.Active)
                .ToListAsync();

            if (subastasFinalizadas.Count == 0)
            {
                return;
            }

            foreach (var subasta in subastasFinalizadas)
            {
                subasta.CurrentState = AuctionState.Closed; // Cambiar el estado de la subasta a cerrada

                // Desactivar todos los productos de la subasta
                var productos = await _dbContext.Products.Where(p => p.AuctionId == subasta.Id).ToListAsync();

                await processProducts(productos);

            }
            // Guardar cambios en la base de datos
            await _dbContext.SaveChangesAsync();
        }

        public async Task processProducts(List<Product> products) {
            try
            {
                foreach (var producto in products)
                {

                    // Determinar el ganador si el producto tiene ofertas
                    var mejorOferta = await _dbContext.Bids
                        .Where(o => o.ProductId == producto.Id)
                        .OrderByDescending(o => o.Price)
                        .FirstOrDefaultAsync();

                    if (mejorOferta != null) // venta
                    {
                        producto.BuyerId = mejorOferta.BidderId; // Asignar el ganador del producto
                        producto.CurrentState = ProductState.Sold;
                        producto.FinalPrice = producto.FinalPrice;
                        await SaleProductAsync(mejorOferta);
                    }
                    else
                    {
                        producto.CurrentState = ProductState.Disabled; // Cambiar el estado del producto a Desactivado

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString(), ex.Message);
            }
            
        }

        public async Task ProcessPendingAuctions()
        {
            var pendingAuctions = await _dbContext.Auctions
                .Where(s => s.StartDate <= DateTime.Now && s.CurrentState == AuctionState.Pending)
                .ToListAsync();
            if (pendingAuctions.Count == 0)
            {
                return;
            }
            foreach (var auction in pendingAuctions)
            {
                //var hasProducts = localContext.Products.Any(p => p.AuctionId == auction.Id);
                var products = await _dbContext.Products.Where(p => p.Id == auction.Id).ToListAsync();
                if (products.Count > 0) // activar si tiene productos
                {
                    auction.CurrentState = AuctionState.Active;
                    // activar productos // SE DEBEN ACTIVAR MANUALMENTE
                    //foreach (var p in products)
                    //{
                    //    p.CurrentState = ProductState.Active;
                    //}
                }
                else
                {
                    auction.CurrentState = AuctionState.Canceled;  // nuevo estado "WaitingForProduct"
                }
            }
            // Guardar cambios en la base de datos
            await _dbContext.SaveChangesAsync();
        }

        public async Task SaleProductAsync( Bid b)
        {
            try
            {
                var newSale = new Sale()
                {
                    BuyerId = (int)b.BidderId,
                    ProductId = (int)b.ProductId,
                    Amount = (float)b.Price,
                    Deduccion = (float)b.Price * 0.10f, // calculo del 10%
                    PaymentMethod = b.PaymentMethods,
                    SaleDate = DateTime.Now,     
                };
                await _dbContext.Sales.AddAsync(newSale);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(message: "No se pudo realizar la venta", ex);
            }
        }
    }
}
