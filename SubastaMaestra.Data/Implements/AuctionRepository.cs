using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SubastaMaestra.Data.Interfaces;
using SubastaMaestra.Data.SubastaMaestra.Data;
using SubastaMaestra.Entities.Core;
using SubastaMaestra.Entities.Enums;
using SubastaMaestra.Models.DTOs;
using SubastaMaestra.Models.DTOs.Auction;
using SubastaMaestra.Models.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Data.Implements
{

    public class AuctionRepository : IAuctionRepository
    {
        private readonly SubastaContext _context;

        public AuctionRepository(SubastaContext context)
        {
            _context = context;
        }

        // Crear una nueva subasta
        public async Task<int> CreateAuctionAsync(AuctionCreateDTO auctionCreateDTO)
        {
            // impedir crear con fecha pasada
            // actualizar el estado
            var auction = new Auction
            {
                Title = auctionCreateDTO.Title,
                FinishDate = auctionCreateDTO.FinishDate,
                StartDate = DateTime.UtcNow,
                CurrentState = AuctionState.Pending // por defecto pendiente

            };
            try
            {
                
                await _context.Auctions.AddAsync(auction);
                await _context.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        // Cerrar una subasta (actualizar su estado)
        public async Task<int> CloseAuctionAsync(int id_subasta)
        {

            try
            {
                var subasta = await _context.Auctions.FindAsync(id_subasta);
                if (subasta == null)
                {
                    return 0;
                }

                subasta.CurrentState = AuctionState.Closed;  // 2 = cerrada o deshabilitada
                subasta.FinishDate = DateTime.Now;  // Actualizar la fecha de cierre a la actual
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        // Modificar una subasta existente
        public async Task<int> EditAuctionAsync(Auction subasta)
        {


            try
            {
                var subastaExistente = await _context.Auctions.FindAsync(subasta.Id);
                if (subastaExistente == null)
                {
                    return 0;
                }

                _context.Entry(subastaExistente).CurrentValues.SetValues(subasta);
                await _context.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        // Obtener subastas abiertas (Estado = 1)
        public async Task<List<AuctionDTO>> GetAllOpenAuctionAsync()
        {
            try
            {
                var today = DateTime.UtcNow;
                var auctions = await _context.Auctions
                                     .Where(s => s.CurrentState == AuctionState.Active && s.FinishDate< today )  // subasta habilitada o abierta
                                     .ToListAsync();
                
                var auctionsDTO = new List<AuctionDTO>();
                foreach(var item in auctions)
                {
                    auctionsDTO.Add(new AuctionDTO
                    {
                        Title = item.Title,
                        FinishDate = item.FinishDate,
                        StartDate = item.StartDate,
                        State = item.CurrentState
                    });
                }
                return auctionsDTO;
            }
            catch (Exception ex)
            {
                return new List<AuctionDTO>();
            }
        }
        public async Task<AuctionDTO> GetAuctionByIdAsync(int id)
        {
            try
            {
                var auction = await _context.Auctions.FirstOrDefaultAsync(s => s.Id == id);
                var auctionDTO = new AuctionDTO
                {
                    Title = auction.Title,
                    FinishDate = auction.FinishDate,
                    StartDate = auction.StartDate,
                    State = auction.CurrentState,
                    Products = new List<ProductDTO>()
                };
                var products = await _context.Products.Where(p => p.AuctionId == auction.Id).ToListAsync();
                foreach (var product in products)
                {
                    auctionDTO.Products.Add(new ProductDTO
                    {
                        AuctionId = product.AuctionId,
                        CategoryId = (int)product.CategoryId,
                        DeliveryCondition = product.DeliveryCondition,
                        BuyerId = (int)product.BuyerId,
                        SellerId = product.SellerId,
                        Condition = product.Condition,
                        CreatedAt = product.CreatedAt,
                        Description = product.Description,
                        FinalPrice = product.FinalPrice,
                        ImgUrl = product.ImgUrl,
                        InitialPrice = product.InitialPrice,
                        Name = product.Name,
                        NumberOfOffers = product.NumberOfOffers

                    });
                }
                
                return auctionDTO;
            }
            catch (Exception ex)
            {
                return new AuctionDTO();
            }

        }

        // Obtener subastas cerradas (Estado = 2)

        public async Task<List<Auction>> GetAllClosedAuctionAsync()
        {
            try
            {
                return await _context.Auctions
                                     .Where(s => s.CurrentState == AuctionState.Closed)  // subasta cerrada
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<Auction>();
            }
        }

        // Obtener subastas cerradas junto con sus productos
        public async Task<List<Auction>> ObtenerSubastasCerradasConProductos()
        {
            try
            {
                return await _context.Auctions
                                     .Where(s => s.CurrentState == AuctionState.Closed)  // 2 = subasta cerrada
                                     .Include(s => s.Products)   // Incluir los productos de cada subasta
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<Auction>();
            }
        }

        // Obtener todas las subastas
        public async Task<List<Auction>> ObtenerTodasSubastas()
        {
            try
            {
                ActualizarEstadoSubastas(); // cada que se consulta las subastas

                return await _context.Auctions
                                     .Include(s => s.Products)   // Incluir productos asociados
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<Auction>();
            }
        }

        public async void ActualizarEstadoSubastas()
        {
            var subastasParaCerrar = await _context.Auctions
                    .Where(s => s.CurrentState == AuctionState.Active && s.FinishDate <= DateTime.UtcNow)
                    .ToListAsync();

            foreach (var subasta in subastasParaCerrar)
            {
                subasta.CurrentState = AuctionState.Closed;  // Cerrar subasta
                //subasta.FinishDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }
    }


}
