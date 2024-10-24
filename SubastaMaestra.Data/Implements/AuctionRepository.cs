using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SubastaMaestra.Data.Interfaces;
using SubastaMaestra.Data.SubastaMaestra.Data;
using SubastaMaestra.Entities.Core;
using SubastaMaestra.Entities.Enums;
using SubastaMaestra.Models.DTOs;
using SubastaMaestra.Models.DTOs.Auction;
using SubastaMaestra.Models.DTOs.Product;
using SubastaMaestra.Models.Utils;
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
        private readonly IMapper _mapper;

        public AuctionRepository(SubastaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Crear una nueva subasta
        public async Task<OperationResult<AuctionCreateDTO>> CreateAuctionAsync(AuctionCreateDTO auctionCreateDTO)
        {
           
            var auction = _mapper.Map<Auction>(auctionCreateDTO);
            auction.CurrentState = AuctionState.Pending;
            auction.StartDate= auctionCreateDTO.StartDate.Date.AddHours(0); // inicia a la 00:00
            auction.FinishDate = auctionCreateDTO.FinishDate.Date.AddHours(0); // inicia a la 00:00

            try
            {
                
                await _context.Auctions.AddAsync(auction);
                await _context.SaveChangesAsync();
                return new OperationResult<AuctionCreateDTO> { Success = true, Message="Suabasta creada con exito"};
            }
            catch (Exception ex)
            {
                return new OperationResult<AuctionCreateDTO> { Success = false, Message = "Error al crear la subasta" };
            }
        }

        // Cerrar una subasta (actualizar su estado)
        public async Task<OperationResult<int>> CloseAuctionAsync(int id_subasta)
        {

            try
            {
                var subasta = await _context.Auctions.FindAsync(id_subasta);
                if (subasta == null)
                {
                    return new OperationResult<int> { Success= false, Message= "Subasta no encontrada", Value= -1};
                }

                subasta.CurrentState = AuctionState.Closed;  // 2 = cerrada o deshabilitada
                subasta.FinishDate = DateTime.Now;  // Actualizar la fecha de cierre a la actual
                // desactivar productos
                var op= await _context.SaveChangesAsync();

                return new OperationResult<int> { Success = true, Message = "Subasta cerrada correctamente", Value = 1 };


            }
            catch (Exception ex)
            {
                return new OperationResult<int> { Success = false, Message = "Error al cerrar la subasta.", Value = -1 };

            }
        }

        // Modificar una subasta existente
        public async Task<OperationResult<int>> EditAuctionAsync(AuctionDTO subasta,int Id)
        {


            try
            {
                var subastaExistente = await _context.Auctions.FindAsync(subasta.Id);
                if (subastaExistente == null)
                {
                    return new OperationResult<int> { Success = false, Message = "Subasta no encontrada.", Value = 0 };
                }
                //auction.StartDate= auctionCreateDTO.StartDate.Date.AddHours(0);

                _context.Entry(subastaExistente).CurrentValues.SetValues(subasta);
                await _context.SaveChangesAsync();
                return new OperationResult<int> { Success = true, Message = "Subasta actualizada con éxito.", Value = 1 };

            }
            catch (Exception ex)
            {
                return new OperationResult<int> { Success = false, Message = "Error al actualizar la subasta.", Value = -1 };

            }
        }
        // Obtener todas las subastas
        public async Task<OperationResult<List<AuctionDTO>>> GetAllAuctionsAsync()
        {
            try
            {
                // cada que se consulta las subastas
               // ActualizarEstadoSubastas();
                var auctions = await _context.Auctions
                                     .Include(s => s.Products)   // Incluir productos asociados
                                     .ToListAsync();
                List<AuctionDTO> auctionsDTO = new List<AuctionDTO>();
                foreach (var auction in auctions)
                {
                    var auc = _mapper.Map<AuctionDTO>(auction);
                     auc.State = auction.CurrentState; // falla el automaper
                    auctionsDTO.Add(auc);

                }
                return new OperationResult<List<AuctionDTO>> { Success= true, Value= auctionsDTO};
            }
            catch (Exception ex)
            {
                return new OperationResult<List<AuctionDTO>> { Success = false, Message="Error al buscar las subastas" };

            }
        }

        // Obtener subastas abiertas (Estado = 1)
        public async Task<OperationResult<List<AuctionDTO>>> GetAllOpenAuctionAsync()
        {
            try
            {
                var today = DateTime.Now;
                var auctions = await _context.Auctions
                                     .Where(s => s.CurrentState == AuctionState.Active && s.FinishDate> today )  // subasta habilitada o abierta
                                     .ToListAsync();
                if( auctions.Count == 0)
                {
                    return new OperationResult<List<AuctionDTO>>{ Success = false, Message = "No hay subastas abiertas." };

                }

                var auctionsDTO = new List<AuctionDTO>();
                foreach(var item in auctions)
                {
                    auctionsDTO.Add(new AuctionDTO
                    {
                        Title = item.Title,
                        FinishDate = item.FinishDate,
                        StartDate = item.StartDate,
                        State = item.CurrentState,
                        Id = item.Id
                        
                    });
                }
                return new OperationResult<List<AuctionDTO>> { Success = true, Value = auctionsDTO };

            }
            catch (Exception ex)
            {
                return new OperationResult<List<AuctionDTO>>  { Success = false, Message = "Error a buscar las subastas" };

            }
        }
        public async Task<OperationResult<AuctionDTO>> GetAuctionByIdAsync(int id)
        {
            try
            {
                var auction = await _context.Auctions.FirstOrDefaultAsync(s => s.Id == id);
                if( auction == null)
                {
                    return new OperationResult<AuctionDTO> { Success = false, Message= "No se encontró la subasta." };

                }

                var auctionDTO = _mapper.Map<AuctionDTO>(auction);
                var products = await _context.Products.Where(p => p.AuctionId == auction.Id).ToListAsync();
                foreach (var product in products)
                {
                    var productDTO = _mapper.Map<ProductDTO>(product);
                    auctionDTO.Products.Add(productDTO);
                    
                }
                
                return new OperationResult<AuctionDTO> { Success = true, Value=auctionDTO };
            }
            catch (Exception ex)
            {
                return new OperationResult<AuctionDTO> { Success = false, Message="Error al buscar la subasta." };

            }

        }



        public async Task<OperationResult<List<AuctionDTO>>> GetAllClosedAuctionAsync()
        {
            try
            {
               var auctions = await _context.Auctions
                                     .Where(s => s.CurrentState == AuctionState.Closed)  // subasta cerrada
                                     .ToListAsync();

                if(auctions == null)
                {
                    return new OperationResult<List<AuctionDTO>> { Success = false, Message = "No hay subastas cerradas." };

                }
                var auctionsDTO = new List<AuctionDTO>();
                foreach (var item in auctions)
                {
                    auctionsDTO.Add(new AuctionDTO
                    {
                        Title = item.Title,
                        FinishDate = item.FinishDate,
                        StartDate = item.StartDate,
                        State = item.CurrentState
                    });
                }

                return new OperationResult<List<AuctionDTO>> { Success = true, Value= auctionsDTO };


            }
            catch (Exception ex)
            {
                return new OperationResult<List<AuctionDTO>> { Success = false, Message = "Error al buscar subastas" };

            }
        }

        // Obtener subastas cerradas junto con sus productos
        public async Task<OperationResult<List<AuctionDTO>>> GetAllClosedAuctionWithProductsAsync()
        {
            try
            {
                 var auctions = await _context.Auctions
                                     .Where(s => s.CurrentState == AuctionState.Closed)  // 2 = subasta cerrada
                                     .Include(s => s.Products)   // Incluir los productos de cada subasta
                                     .ToListAsync();
                if (auctions == null)
                {
                    return new OperationResult<List<AuctionDTO>> { Success = false, Message="No se encontraron subastas" };
                }
                var auctionsDTO = new List<AuctionDTO>();
                foreach (var item in auctions)
                {
                    auctionsDTO.Add(_mapper.Map<AuctionDTO>(item));
                }
                return new OperationResult<List<AuctionDTO>> { Success = true, Value = auctionsDTO };
            }
            catch (Exception ex)
            {
                return new OperationResult<List<AuctionDTO>> { Success = false, Message = "Error al buscar subastas" };

            }
        }

       

        public async void ActualizarEstadoSubastas()
        {
            var toCloseAuctions =  await _context.Auctions
                    .Where(s => s.CurrentState == AuctionState.Active && s.FinishDate <= DateTime.Now)
                    .ToListAsync();

            foreach (var subasta in toCloseAuctions)
            {
                subasta.CurrentState = AuctionState.Closed;  // Cerrar subasta
                //subasta.FinishDate = DateTime.UtcNow;
                if (subasta.Products.Count()>0)
                {
                    foreach (var producto in subasta.Products)// desabilito los productos 
                    {
                        producto.CurrentState= ProductState.Disabled;
                    }
                }
                
                
            }

            await _context.SaveChangesAsync();
        }
    }


}
