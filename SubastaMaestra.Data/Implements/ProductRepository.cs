using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SubastaMaestra.Data.Interfaces;
using SubastaMaestra.Data.SubastaMaestra.Data;
using SubastaMaestra.Entities.Core;
using SubastaMaestra.Entities.Enums;
using SubastaMaestra.Models.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Data.Implements
{
    public class ProductRepository : IProductRepository
    {
        private readonly SubastaContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(SubastaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Crear un nuevo producto
        public async Task<int> CreateProductAsync(ProductCreateDTO productCreateDTO)
        {
            //var product = new Product{
            //    Name = productCreateDTO.Name,
            //    AuctionId = productCreateDTO.AuctionId,
            //    CreatedAt = DateTime.UtcNow,
            //    Condition = productCreateDTO.Condition,
            //    InitialPrice = productCreateDTO.InitialPrice,
            //    ImgUrl = productCreateDTO.ImgUrl,
            //    Description = productCreateDTO.Description,
            //    DeliveryCondition = productCreateDTO.DeliveryCondition,
            //    SellerId = productCreateDTO.SellerId,   
            //    FinalPrice = 0, // por defecto  
            //    CurrentState = ProductState.Pending, // pendiente
            //    NumberOfOffers = 0,
            //    CategoryId = productCreateDTO.CategoryId,
               // PaymentMethod = productCreateDTO.PaymentMethod

            //};
            var product = _mapper.Map<Product>(productCreateDTO);
            try
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                return -1;  // Devuelve -1 si ocurre un error
            }
        }

        // Obtener un producto por su ID
        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {


            try
            {
                var product = await _context.Products
                                     .Include(p => p.Category)
                                     .Include(p => p.Seller)
                                     .Include(p => p.Auction)
                                     .FirstOrDefaultAsync(p => p.Id == id);
                if (product == null) {
                    return null;
                }
                var productDTO = new ProductDTO
                {
                    AuctionId = product.AuctionId,
                    Name = product.Name,
                    CategoryId = (int)product.CategoryId,
                    SellerId = product.SellerId,
                    CreatedAt = product.CreatedAt,
                    Condition = product.Condition,
                    DeliveryCondition = product.DeliveryCondition,
                    Description = product.Description,
                    FinalPrice = product.FinalPrice,
                    ImgUrl = product.ImgUrl,
                    InitialPrice = product.InitialPrice,
                    NumberOfOffers = product.NumberOfOffers
                };
                return productDTO ;
                        
            }
            catch (Exception ex)
            {
                
                return null;
            }
        }

        // Obtener todos los productos
        public async Task<List<ProductDTO>> GetAllProductsAsync()
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
                    return new List<ProductDTO>();

                }
                var productsDTO = new List<ProductDTO>();
                foreach (var p in productos)
                {
                    productsDTO.Add(_mapper.Map<ProductDTO>(p));
                }
                
                return productsDTO;
            }
            catch (Exception ex)
            {
                return new List<ProductDTO>();
            }
        }

        // Obtener productos activos (en venta)
        public async Task<List<Product>> GetActiveProductsAsync()
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
                    return new List<Product>();
                }
                return productos;
            }
            catch (Exception ex)
            {
                return new List<Product>();
            }
        }

       

        // Obtener productos por subasta
        public async Task<List<Product>> GetProductsByBidAsync(int id_subasta)
        {


            try
            {
                var producto = await _context.Products
                                     .Where(p => p.Id == id_subasta)
                                     .Include(p => p.Category)
                                     .Include(p => p.Seller)
                                     .ToListAsync();
                if (producto == null)
                {
                    return new List<Product>();
                }
                return producto;
            }
            catch (Exception ex)
            {
                return new List<Product>();
            }
        }

        // Deshabilitar un producto (cambiar estado a inactivo)
        public async Task<int> DisableProductAsync(int id)
        {

            try
            {
                var producto = await _context.Products.FindAsync(id);
                if (producto == null)
                {
                    return 0; //no encontrado
                }

                producto.CurrentState = ProductState.Disabled;  // 2 = deshabilitado
                await _context.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        // Habilitar un producto (cambiar estado a activo)
        public async Task<int> EnableProductAsync(int id)
        {

            try
            {
                var producto = await _context.Products.FindAsync(id);
                if (producto == null)
                {
                    return 0;
                }

                producto.CurrentState = ProductState.Active;  // 1 = habilitado
                await _context.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        // Editar un producto
        public async Task<int> EditProductAsync(Product producto)
        {
          
            try
            {
                var productoExistente = await _context.Products.FindAsync(producto.Id);
                if (productoExistente == null)
                {
                    return 0;
                }

                _context.Entry(productoExistente).CurrentValues.SetValues(producto);
                await _context.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }


    }
}
