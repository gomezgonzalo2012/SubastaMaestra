using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SubastaMaestra.Data.Interfaces;
using SubastaMaestra.Data.SubastaMaestra.Data;
using SubastaMaestra.Entities.Core;
using SubastaMaestra.Models.DTOs.Bid;
using SubastaMaestra.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            // Consulta LINQ para obtener la fecha de inicio de la subasta relacionada con el producto
                var auctionFinish = _context.Products
                    .Where(p => p.Id == bidDTO.ProductId)                // Filtrar por el ID del producto
                        .Select(p => p.Auction.FinishDate)           // Navegar a la subasta y seleccionar la fecha de inicio
                            .FirstOrDefault();                            // Obtener el primer (y único) resultado o nulo si no existe
            if (auctionFinish < DateTime.Now)
            {
                return new OperationResult<BidCreateDTO> { Success = false, Message = "La subasta ya cerró." };

            }
            var bid = _mapper.Map<Bid>(bidDTO);
            bid.OfferDate = DateTime.Now;
            bid.Price = (float)bidDTO.Amount;
            try
            {
                await _context.Bids.AddAsync(bid);
                await _context.SaveChangesAsync();
                return new OperationResult<BidCreateDTO> { Success = true, Message = "Oferta creada correctamente", Value= bidDTO };
            }
            catch (Exception ex)
            {

                return new OperationResult<BidCreateDTO> { Success = false, Message = "Error al crear la oferta" };
            }
        }

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

       
    }
}

