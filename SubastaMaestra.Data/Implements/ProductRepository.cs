using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SubastaMaestra.Data.Interfaces;
using SubastaMaestra.Data;
using SubastaMaestra.Entities.Core;
using SubastaMaestra.Entities.Enums;
using SubastaMaestra.Models.DTOs.Product;
using SubastaMaestra.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubastaMaestra.Models.DTOs;
using SubastaMaestra.Models.DTOs.Reports;

namespace SubastaMaestra.Data.Implements
{
    public class ProductRepository : IProductRepository
    {
        private readonly SubastaContext _context;

        private readonly IMapper _mapper;
        private readonly INotificationRepository _notificationRepository;


        public ProductRepository(SubastaContext context, IMapper mapper, INotificationRepository notificationRepository)
        {
            _context = context;
            _mapper = mapper;
            _notificationRepository = notificationRepository;
        }

        // Crear un nuevo producto
        public async Task<OperationResult<ProductCreateDTO>> CreateProductAsync(ProductCreateDTO productCreateDTO)
        {
            //metodo de servicio
            var auction = _context.Auctions.Where(a=>a.Id==productCreateDTO.AuctionId).FirstOrDefault();
            if (auction != null)
            {
                if (auction.FinishDate <= DateTime.Now||auction.CurrentState==AuctionState.Closed
                    || auction.CurrentState == AuctionState.Canceled)
                {
                    return new OperationResult<ProductCreateDTO> { Success = false, Message = "La subasta no se encuentra habilitada " };
                }
            }
            var dbproduct = _mapper.Map<Product>(productCreateDTO);
            dbproduct.CreatedAt = DateTime.Now;
            try
            {
                await _context.Products.AddAsync(dbproduct);
                await _context.SaveChangesAsync();
                if(dbproduct.Id != 0)
                {
                    return new OperationResult<ProductCreateDTO> { Success = true, Message = "Producto creado correctamente", Value = productCreateDTO };
                }
                else
                {
                    return new OperationResult<ProductCreateDTO> { Success = false, Message = "No creado" };

                }
            }
            catch (Exception ex)
            {
                return new OperationResult<ProductCreateDTO> { Success = false, Message = "Error al crear el Producto" };
                
            }
        }

        // Obtener un producto por su ID
        public async Task<OperationResult<ProductDTO>> GetProductByIdAsync(int id)
        {


            try
            {
                var product = await _context.Products
                                     .Include(p => p.Category)
                                     .Include(p => p.Seller)
                                     .Include(p => p.Auction)
                                     .FirstOrDefaultAsync(p => p.Id == id);
                //var buyer = await _context.Users.Where(u=>u.Id == product.BuyerId).FirstOrDefaultAsync();
                if (product == null) {
                    return new OperationResult<ProductDTO> { Success=false, Message= "Producto no encontrado."};
                }
                var productDTO = _mapper.Map<ProductDTO>(product);

                return new OperationResult<ProductDTO> { Success = true, Value= productDTO };
                        
            }
            catch (Exception ex)
            {
               return new OperationResult<ProductDTO> { Success = false, Message = "Error al buscar el producto" };
            }
        }

        // productos sin ofertas
        public async Task<OperationResult<List<ProductDTO>>> GetProductsWithoutBids()
        {
            try
            {
                // Obtener productos sin ofertas
                var productosSinOfertas = await _context.Products
                    .Where(p => !_context.Bids.Any(b => b.ProductId == p.Id) && p.CurrentState == ProductState.Disabled)
                    .ToListAsync();

                if (productosSinOfertas == null)
                {
                    return new OperationResult<List<ProductDTO>> { Success = false, Message = "Productos no encontrados." };
                }
                var productsDTO = new List<ProductDTO>();
                foreach( var product in productosSinOfertas)
                {
                    productsDTO.Add(_mapper.Map<ProductDTO>(product));
                }
                return new OperationResult<List<ProductDTO>> { Success = true, Value = productsDTO };

            }
            catch (Exception ex)
            {
                return new OperationResult<List<ProductDTO>>{ Success = false, Message = "Error al buscar los productos" };
            }
        }

        // Obtener todos los productos
        public async Task<OperationResult<List<ProductDTO>>> GetAllProductsAsync()
        {
            try
            {
                var productos = await _context.Products
                                     .Include(p => p.Category)
                                     .Include(p => p.Seller)
                                     .Include(p => p.Auction)
                                     .ToListAsync();

                if (productos == null)
                {
                    return new OperationResult<List<ProductDTO>> { Success = false, 
                        Message = "No se pudieron encontrar productos."};

                }
                var productsDTO = new List<ProductDTO>();
                foreach (var p in productos)
                {
                    productsDTO.Add(_mapper.Map<ProductDTO>(p));
                }
                
                return  new OperationResult<List<ProductDTO>>
                {
                    Success = true,
                    Value = productsDTO
                }; 
            }
            catch (Exception ex)
            {
                return new OperationResult<List<ProductDTO>>
                {
                    Success = false,
                    Message = ex.Message,
                };

            }
        }

        // Obtener productos activos (en venta)
        public async Task<OperationResult<List<ProductDTO>>> GetActiveProductsAsync()
        {
            try
            {
                var productos = await _context.Products
                                      .Where(p => p.CurrentState == ProductState.Active)  // Estado habilitado
                                      .Include(p => p.Category)
                                      .Include(p => p.Seller)
                                      .Include(p => p.Auction)
                                      .ToListAsync();

                if (productos == null)
                {
                    return new OperationResult<List<ProductDTO>>
                    {
                        Success = false,
                        Message = "No se pudieron encontrar productos.",
                    };

                }
                var productsDTO = new List<ProductDTO>();
                foreach (var p in productos)
                {
                    productsDTO.Add(_mapper.Map<ProductDTO>(p));
                }

                return new OperationResult<List<ProductDTO>>
                {
                    Success = true,
                    Value = productsDTO
                };
            }
            catch (Exception ex)
            {
                return new OperationResult<List<ProductDTO>>
                {
                    Success = false,
                    Message = ex.Message,
                };

            }
        }
        // Obtener productos por subasta
    public async Task<OperationResult<List<ProductDTO>>> GetProductsByAuctionAsync(int id_subasta)
    {
            try
            {
                var products = await _context.Products
                                      .Where(p => p.AuctionId == id_subasta)
                                      .Include(p => p.Category)
                                      .Include(p => p.Seller)
                                      .Include(p => p.Buyer)
                                      .ToListAsync();

                if (products == null)
                {
                    return new OperationResult<List<ProductDTO>>
                    {
                        Success = false,
                        Message = "No se pudieron encontrar productos.",
                    };

                }
                var productsDTO = new List<ProductDTO>();
                foreach (var p in products)
                {
                    productsDTO.Add(_mapper.Map<ProductDTO>(p));
                }

                return new OperationResult<List<ProductDTO>>
                {
                    Success = true,
                    Value = productsDTO
                };
            }
            catch (Exception ex)
            {
                return new OperationResult<List<ProductDTO>>
                {
                    Success = false,
                    Message = ex.Message,
                    Value = null
                };

            }
    }

        // Deshabilitar un producto (cambiar estado a inactivo)
        public async Task<OperationResult<int>> DisableProductAsync(int id)
        {

            try
            {
                var producto = await _context.Products.FindAsync(id);
                if (producto == null)
                {
                    return new OperationResult<int> { Success = false, Message = "El producto no existe" };
                }

                producto.CurrentState = ProductState.Disabled;  // 1 = habilitado
                await _context.SaveChangesAsync();
                // crear notificaion de producto habilitado
                await _notificationRepository.CreateNotification(producto.SellerId, producto.Id, NotificationType.RejectedNotification);
                return new OperationResult<int> { Success = true, Message = "Producto Deshabilidato" };
            }
            catch (Exception ex)
            {
                return new OperationResult<int> { Success = false, Message = "Error al deshabilitar el producto " };
            }
        }

        // Habilitar un producto (cambiar estado a activo)
        public async Task<OperationResult<int>> EnableProductAsync(int id)
        { 
            try
            {
                var producto = await _context.Products.FindAsync(id);
               
                if (producto == null)
                {
                    return new OperationResult<int> { Success = false, Message = "El producto no existe" };
                }
                if (producto.AuctionId == null)
                {
                    return new OperationResult<int> { Success = false, Message = "El producto no tiene subasta asignada" };
                }
                producto.CurrentState = ProductState.Active;  // 1 = habilitado
                await _context.SaveChangesAsync();
                // crear notificaion de producto habilitado
                await _notificationRepository.CreateNotification(producto.SellerId, producto.Id, NotificationType.AcceptedNotification);

                return new OperationResult<int> { Success = true, Message = "Producto Habilidato" };
            }
            catch (Exception ex)
            {
                return new OperationResult<int> { Success = false, Message = "Error al habilitar el producto " };
            }
        }

        // Editar un producto
        public async Task<OperationResult<int>> EditProductAsync(ProductDTO producto, int id)
        {
          
            try
            {
                var productoExistente = await _context.Products.FindAsync(producto.Id);
                if (productoExistente == null)
                {
                    return new OperationResult<int> { Success = false, Message = "El producto no coincide." };
                }

                _context.Entry(productoExistente).CurrentValues.SetValues(producto);
                await _context.SaveChangesAsync();
                return new OperationResult<int> { Success = true, Message = "Producto editado correctamente" };
               
            }
            catch (Exception ex)
            {
                    return new OperationResult <int>{ Success = false, Message = "Error al editar el producto " };

            }
        }

        public async Task<OperationResult<List<ProductDTO>>> GetProductsBySeller(int seller_Id)
        {
            try
            {
                var products = await _context.Products.Where(p => p.SellerId == seller_Id).Include(p=>p.Auction).ToListAsync();
                if(products.Count == 0 || products == null)
                {
                    return new OperationResult<List<ProductDTO>> { Success = true, Message = "Sin productos", Value = new List<ProductDTO>() };
                }
                var productsDTO = new List<ProductDTO>();
                products.ForEach(p => productsDTO.Add(_mapper.Map<ProductDTO>(p)));
                //foreach (var product in products)
                //{
                //    var p = _mapper.Map<ProductDTO>(product);
                //    productsDTO.Add(p);
                //}
                    
                 return new OperationResult<List<ProductDTO>> { Success = true, Value = productsDTO };
                
            }
            catch (Exception ex)
            {
                return new OperationResult<List<ProductDTO>> { Success = true,Message=$"Error al recuperar productos {ex.Message}" ,Value = null };

            }
        }
    }
}
