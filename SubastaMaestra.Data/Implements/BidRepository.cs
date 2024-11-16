using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SubastaMaestra.Data.Interfaces;
using SubastaMaestra.Data;
using SubastaMaestra.Entities.Core;
using SubastaMaestra.Models.DTOs.Bid;
using SubastaMaestra.Models.DTOs.User;
using SubastaMaestra.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubastaMaestra.Models.DTOs.Product;
using SubastaMaestra.Models.DTOs.Auction;

namespace SubastaMaestra.Data.Implements
{
    public class BidRepository : IBidRepository
    {
        private readonly SubastaContext _context;
        private readonly IMapper _mapper;
        public BidRepository(SubastaContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OperationResult<BidCreateDTO>> CreateBid(BidCreateDTO bidDTO)
        {
            // validar que el producto este activo y no vendido
            // validar que no se pueda ofertar menos del minimo
            var product = _context.Products.FirstOrDefault(p=>p.Id== bidDTO.ProductId);
            if (product == null)
            {
                return new OperationResult<BidCreateDTO> { Success = false, Message = "El producto no existe" };

            }
            if (product.SellerId == bidDTO.BidderId) 
            {
                return new OperationResult<BidCreateDTO> { Success = false, Message = "El vendedor no puede ofertar por su producto." };
            }
            if (product.InitialPrice >= bidDTO.Amount)
            {
                return new OperationResult<BidCreateDTO> { Success = false, Message = "El monto debe superar el precio mínimo" };
            }
            var bidders = await GetBiddersByProduct(bidDTO.ProductId);
            if(bidders.Value != null)
            {
                foreach (var bidder in bidders.Value)
                {
                    if (bidder.BidderId == bidDTO.BidderId)
                    {
                        return new OperationResult<BidCreateDTO> { Success = false, Message = "El usuaro ya ofertó." };
                    }

                }

            }
            
            // Consulta LINQ para obtener la fecha de inicio de la subasta relacionada con el producto
            var auctionFinish = _context.Products
                    .Where(p => p.Id == bidDTO.ProductId)                // Filtrar por el ID del producto
                        .Select(p => new
                        {
                            Finish = p.Auction.FinishDate, // Seleccionar la fecha de finalización
                            State = p.Auction.CurrentState // Seleccionar el estado actual
                        })          // Navegar a la subasta y seleccionar la fecha de inicio
                            .FirstOrDefault();                            // Obtener el primer (y único) resultado o nulo si no existe
            if (auctionFinish.Finish < DateTime.Now)
            {
                return new OperationResult<BidCreateDTO> { Success = false, Message = "La subasta ya cerró." };

            }

            if (auctionFinish.State == Entities.Enums.AuctionState.Pending)
            {
                return new OperationResult<BidCreateDTO> { Success = false, Message = "La subasta aún no esta disponible para ofertar." };

            }


            var bid = _mapper.Map<Bid>(bidDTO);
            bid.OfferDate = DateTime.Now;
            bid.Price = (float)bidDTO.Amount;
            try
            {
                product.NumberOfOffers++ ;
                await _context.Bids.AddAsync(bid);
                await _context.SaveChangesAsync();
                return new OperationResult<BidCreateDTO> { Success = true, Message = "Oferta creada correctamente", Value= bidDTO };
            }
            catch (Exception ex)
            {

                return new OperationResult<BidCreateDTO> { Success = false, Message = "Error al crear la oferta" };
            }
        }

        // obtener catidad de ofertas
        public async Task<OperationResult<List<BidDTO>>> ObtenerOfertasPorProducto(int id_producto)
        {
            try
            {
                var bids = await _context.Bids.Where(
                    of => of.ProductId == id_producto).ToListAsync();
                
                if (bids.Any())
                {
                    var bidsDTO = new List<BidDTO>();
                    bids.ForEach(b => bidsDTO.Add(_mapper.Map<BidDTO>(b)));
                    return new OperationResult<List<BidDTO>> { Success = true,Value= bidsDTO};
                }
                return new OperationResult<List<BidDTO>> { Success = false, Message = "No hay ofertas." };
            }
            catch (Exception ex)
            {
                return new OperationResult<List<BidDTO>> { Success = false, Message="Error al cargar las ofertas." };

            }
        }

       // obtener oferentes por producto

        public async Task<OperationResult<List<BidderDTO>>> GetBiddersByProduct(int id_product)
        {
            try
            {
                var bidder = await _context.Bids.Where(b => b.ProductId == id_product)
                .Include(b => b.Bidder)
                .Select(b => new BidderDTO
                {
                    BidderId = b.BidderId,
                    Name = b.Bidder.Name,
                    OfferDate = b.OfferDate,
                    Price = b.Price,
                    PaymentMethod = b.PaymentMethods
                })
                .OrderBy(b => b.OfferDate)
                .ToListAsync();
                if(bidder.Count==0)
                {
                    return new OperationResult<List<BidderDTO>> { Success = true, Message = "No hay oferentes por este producto" };

                }
                return new OperationResult<List<BidderDTO>> { Success = true, Value = bidder };

            }
            catch(Exception ex)
            {
                return new OperationResult<List<BidderDTO>> { Success = false, Message = ex.Message };
            }
            
         }

        public async Task<OperationResult<List<BidDTO>>> GetBidsByUser(int bidder_id)
        {
            try
            {
                var bids = await _context.Bids.Where(b => b.BidderId == bidder_id)
                    .Include(b => b.Bidder)
                    .Include (b => b.Product)
                    .ToListAsync();
                if(bids.Count==0 || bids == null)
                {
                    return new OperationResult<List<BidDTO>> { Success = true, Message = "No hay ofertas",Value = new List<BidDTO>() };

                }
                var bidsDTO = new List<BidDTO>();
                foreach (var bid in bids)
                {
                    var b = _mapper.Map<BidDTO>(bid);
                    bidsDTO.Add(b);
                }

                return new OperationResult<List<BidDTO>> { Success = true, Value = bidsDTO };
            }
            catch (Exception ex)
            {
                return new OperationResult<List<BidDTO>> { Success = false, Message = ex.Message };
            }

        }

    }
}

