using SubastaMaestra.Entities.Core;
using SubastaMaestra.Models.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Data.Interfaces
{
    public interface IProductRepository
    {
        Task<int> CreateProductAsync(ProductCreateDTO producto);
        Task<ProductDTO> GetProductByIdAsync(int id);
        Task<List<ProductDTO>> GetAllProductsAsync();
        //Visualizar todos los productos en venta
        Task<List<Product>> GetActiveProductsAsync();

        // ver productos innactivos
        // ver productos ppor subasta
        Task<List<Product>> GetProductsByBidAsync(int id_subasta);
        Task<int> DisableProductAsync(int id);
        Task<int> EnableProductAsync(int id); 
        Task<int> EditProductAsync(Product product);


      
       




    }
}
