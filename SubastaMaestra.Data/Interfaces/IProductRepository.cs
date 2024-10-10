using SubastaMaestra.Entities.Core;
using SubastaMaestra.Models.DTOs.Product;
using SubastaMaestra.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Data.Interfaces
{
    public interface IProductRepository
    {
        Task<OperationResult<ProductCreateDTO>> CreateProductAsync(ProductCreateDTO producto);
        Task<OperationResult<ProductDTO>> GetProductByIdAsync(int id);
        Task<OperationResult<List<ProductDTO>>> GetAllProductsAsync();
        //Visualizar todos los productos en venta
        Task<OperationResult<List<ProductDTO>>> GetActiveProductsAsync();

        // ver productos innactivos
        // ver productos ppor subasta
        Task<OperationResult<List<ProductDTO>>> GetProductsByAuctionAsync(int id_subasta);
        Task<OperationResult<int>> DisableProductAsync(int id);
        Task<OperationResult<int>> EnableProductAsync(int id); 
        Task<OperationResult<int>> EditProductAsync(ProductDTO product, int id);


      
       




    }
}
