using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SubastaMaestra.Data.Interfaces;
using SubastaMaestra.Data;
using SubastaMaestra.Entities.Core;
using SubastaMaestra.Entities.Enums;
using SubastaMaestra.Models.DTOs.Auction;
using SubastaMaestra.Models.DTOs.Product;
using SubastaMaestra.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubastaMaestra.Models.DTOs.Reports;

namespace SubastaMaestra.Data.Implements
{

    public class AuctionRepository : IAuctionRepository
    {
        private readonly SubastaContext _context;
        private readonly IMapper _mapper;
        private readonly AuctionHandlerService _auctionHandlerService;
        public AuctionRepository(SubastaContext context, IMapper mapper, AuctionHandlerService auctionHandlerService)
        {
            _context = context;
            _mapper = mapper;
            _auctionHandlerService = auctionHandlerService;
        }

        // Crear una nueva subasta
        public async Task<OperationResult<AuctionCreateDTO>> CreateAuctionAsync(AuctionCreateDTO auctionCreateDTO)
        {
            if (auctionCreateDTO.StartDate.AddMinutes(5) < DateTime.Now) // validacion de fecha de inicio < fecha actual
            {
                return new OperationResult<AuctionCreateDTO> { Success = false, Message = "La fecha de inicio no puede ser anterior a la fecha actual" };

                
               
            }
            if (auctionCreateDTO.FinishDate < DateTime.Now) // validacion fecha fin > fecha inicio
            {
                return new OperationResult<AuctionCreateDTO> { Success = false, Message = "La fecha de fin debe ser anterior a la fecha  actual." };
            }
            if (auctionCreateDTO.FinishDate <= auctionCreateDTO.StartDate) // validacion fecha fin > fecha inicio
            {
                return new OperationResult<AuctionCreateDTO> { Success = false, Message = "La fecha de fin debe ser posterior a la fecha de inicio." };
                
            }
            if (auctionCreateDTO.StartDate.Date < DateTime.Now.AddDays(3))
            {
                return new OperationResult<AuctionCreateDTO> { Success = false, Message = "La subasta debe tener almenos 3 días de antelación para iniciar." };
            }
            var diff = auctionCreateDTO.FinishDate - auctionCreateDTO.StartDate;
            if (diff < TimeSpan.FromDays(1))
            {
                return new OperationResult<AuctionCreateDTO> { Success = false, Message = "La subasta debe tener almenos 1 día de duración." };
            }

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
        // activar subasta

        public async Task<OperationResult<int>> ActivateAuctionAsync(int id_subasta)
        {
            try
            {
                var subasta = await _context.Auctions.FindAsync(id_subasta);
                if (subasta == null)
                {
                    return new OperationResult<int> { Success = false, Message = "Subasta no encontrada", Value = -1 };
                }
                if(subasta.StartDate> DateTime.Now.AddDays(1)) // solo activar subastas del dia actual
                {
                    return new OperationResult<int> { Success = false, Message = "Subasta con inicio futuro", Value = -1 };

                }
                subasta.CurrentState = AuctionState.Active;  
                // desactivar productos
                var op = await _context.SaveChangesAsync();

                return new OperationResult<int> { Success = true, Message = "Subasta activada correctamente", Value = 1 };


            }
            catch (Exception ex)
            {
                return new OperationResult<int> { Success = false, Message = "Error al activar la subasta.", Value = -1 };

            }
        }
        public async Task<OperationResult<int>> DisableAuctionAsync(int id_subasta)
        {

            try
            {
                var subasta = await _context.Auctions.FindAsync(id_subasta);
                if (subasta == null)
                {
                    return new OperationResult<int> { Success = false, Message = "Subasta no encontrada", Value = -1 };
                }

                subasta.CurrentState = AuctionState.Canceled;  // 2 = cerrada o deshabilitada
                var op = await _context.SaveChangesAsync();

                return new OperationResult<int> { Success = true, Message = "Subasta deshabilitada correctamente", Value = 1 };


            }
            catch (Exception ex)
            {
                return new OperationResult<int> { Success = false, Message = "Error al cerrar la subasta.", Value = -1 };

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
        public async Task<OperationResult<int>> EditAuctionAsync(AuctionUpdateDTO subasta,int id)
        {


            try
            {

                var subastaExistente = await _context.Auctions.FindAsync(id);
                
                if (subastaExistente == null)
                {
                    return new OperationResult<int> { Success = false, Message = "Subasta no encontrada.", Value = 0 };
                }
                if (subastaExistente.CurrentState == AuctionState.Closed)
                {
                    return new OperationResult<int> { Success = false, Message = "La subasta ya ha sido cerrada.", Value = 0 };
                }
                if (subasta.StartDate.AddMinutes(5) < DateTime.Now) // validacion de fecha de inicio < fecha actual
                {
                    return new OperationResult<int> { Success = false, Message = "La fecha de inicio no puede ser anterior a la fecha actual.", Value = 0 };
                }
                if (subasta.FinishDate < DateTime.Now) // validacion de fecha de inicio < fecha actual
                {
                    return new OperationResult<int> { Success = false, Message = "La fecha de cierre no puede ser anterior a la fecha actual.", Value = 0 };
                }
                if (subasta.FinishDate <= subasta.StartDate) // validacion fecha fin > fecha inicio
                {
                    return new OperationResult<int> { Success = false, Message = "La fecha de fin debe ser posterior a la fecha de inicio.", Value = 0 };
                }
                //if (subasta.StartDate.Date < DateTime.Now.AddDays(3))
                //{
                //    return new OperationResult<int> { Success = false, Message = "La subasta debe terner almenos 3 días de antelación para inciar.", Value = 0 };
                //}
                var diff = subasta.FinishDate - subasta.StartDate;
                if (diff < TimeSpan.FromDays(1))
                {
                    return new OperationResult<int> { Success = false, Message = "La subasta debe terner almenos un día de duración", Value = 0 };                  
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
            await _auctionHandlerService.ProcessAuctions(); // acutaliza los estados

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
            await _auctionHandlerService.ProcessAuctions(); // acutaliza los estados

            try
            {
                var today = DateTime.Now;
                var auctions = await _context.Auctions
                                     .Where(s => s.CurrentState == AuctionState.Active && s.FinishDate> today )  // subasta habilitada o abierta
                                     .Include(s => s.Products
                                        .Where(p => p.CurrentState.Equals(ProductState.Active)))
                                         .ThenInclude(p => p.Seller )
                                     .ToListAsync();
                if( auctions.Count == 0)
                {
                    return new OperationResult<List<AuctionDTO>>{ Success = false, Message = "No hay subastas abiertas." };

                }

                List<AuctionDTO> auctionsDTO = new List<AuctionDTO>();
                 foreach (var auction in auctions)
                {
                    var auc = _mapper.Map<AuctionDTO>(auction);
                    auc.State = auction.CurrentState; // falla el automaper
                    //if( auction.products.any())
                    //{
                    //    foreach (var p in auc.products)
                    //    {
                    //        var productdto = _mapper.map<productdto>(p);
                    //        auc.products.add(productdto);

                    //    }
                    //}
                    auctionsDTO.Add(auc);

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
                auctionDTO.State = auction.CurrentState; // falla el automaper
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

        public async Task<OperationResult<List<AuctionDTO>>> GetAllPendingAuctionsAsync()
        {
            try
            {
                var auctions = await _context.Auctions
                                      .Where(s => s.CurrentState == AuctionState.Pending)  // subasta pendiente
                                      .ToListAsync();

                if (auctions == null)
                {
                    return new OperationResult<List<AuctionDTO>> { Success = false, Message = "No hay subastas pendientes." };

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

                return new OperationResult<List<AuctionDTO>> { Success = true, Value = auctionsDTO };


            }
            catch (Exception ex)
            {
                return new OperationResult<List<AuctionDTO>> { Success = false, Message = "Error al buscar subastas" };

            }

        }

        public async Task<OperationResult<List<AuctionDTO>>> GetAllAuctionByCurrentStateAsync( AuctionState currentState)
        {
            await _auctionHandlerService.ProcessAuctions(); // acutaliza los estados

            try
            {
                var auctions = new List<Auction>();
                auctions = await _context.Auctions
                                     .Where(s => s.CurrentState == currentState)  // subasta habilitada o abierta
                                     .Include(s => s.Products)
                                     .ThenInclude(p => p.Seller)
                                     .ToListAsync();
                if (auctions.Count == 0)
                {
                    return new OperationResult<List<AuctionDTO>> { Success = false, Message = "No hay subastas "+ currentState.ToString(), Value = new List<AuctionDTO>() };

                }

                List<AuctionDTO> auctionsDTO = new List<AuctionDTO>();
                foreach (var auction in auctions)
                {
                    var auc = _mapper.Map<AuctionDTO>(auction);
                    auc.State = auction.CurrentState; // falla el automaper
                  
                    auctionsDTO.Add(auc);

                }
                return new OperationResult<List<AuctionDTO>> { Success = true, Value = auctionsDTO };

            }
            catch (Exception ex)
            {
                return new OperationResult<List<AuctionDTO>> { Success = false, Message = "Error a buscar las subastas" };
            }
        }

        public async Task<List<AuctionReportDTO>> ObtenerSubastasMasPopulares(DateTime inicio, DateTime fin)
        {
            // Obtener las subastas filtradas y calcular la cantidad de ofertas
            var subastasMasPopulares = await _context.Auctions
                .Where(a => a.CurrentState == AuctionState.Closed &&
                            a.FinishDate >= inicio && a.FinishDate <= fin)
                .Select(a => new
                {
                    a.Id,
                    a.Title,
                    a.FinishDate,
                    // Calcular el total de ofertas sumando las ofertas de cada producto
                    TotalOfertas = a.Products.SelectMany(p => p.Bids).Count(),
                    // Calcular el monto máximo de todas las ofertas de los productos
                    HighestBidAmount = a.Products
                        .SelectMany(p => p.Bids)
                        .Max(b => (decimal?)b.Price) // Usar (decimal?) para manejar subastas sin ofertas
                        ?? 0 // Si no hay ofertas, el monto máximo será 0
                })
                .OrderByDescending(a => a.TotalOfertas) // Ordenar por cantidad de ofertas
                .Take(15) // Tomar las 15 subastas con mayor cantidad de ofertas
                .ToListAsync();

            // Transformar a DTO para la salida
            return subastasMasPopulares.Select(a => new AuctionReportDTO
            {
                AuctionId = a.Id,
                Title = a.Title,
                FinishDate = a.FinishDate,
                TotalOfertas = a.TotalOfertas,
                HighestBidAmount = a.HighestBidAmount
            }).ToList();
        }

        public async Task<List<ProfitReportDTO>> GetProfitReport()
        {
                    var subastasConVentas = await _context.Auctions
            .Where(a => a.Products.Any(p => p.Sales.Any()) /*&& (a.FinishDate <= DateTime.Now && a.StartDate >= DateTime.Now.AddDays(-30))*/) // Filtrar subastas que tienen productos vendidos
            .Select(subasta => new ProfitReportDTO
            {
                AuctionId = subasta.Id,
                AuctionTitle = subasta.Title,
                StartDate = subasta.StartDate,
                FinishDate = subasta.FinishDate,
                TotalProductosVendidos = subasta.Products.Where(p => p.Sales.Any()).Count(),
                MontoTotalVentas = subasta.Products.SelectMany(p => p.Sales).Sum(s => (float?)s.Amount) ?? 0,
                GananciaTotal = subasta.Products.SelectMany(p => p.Sales).Sum(s => (float?)s.Deduccion) ?? 0
            })
            .OrderByDescending(x => x.FinishDate) //ordenar por monto total de ventas
            .ToListAsync();

            return subastasConVentas;
        }
    }


}
