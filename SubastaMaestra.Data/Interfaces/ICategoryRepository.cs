using SubastaMaestra.Entities.Core;
using SubastaMaestra.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Data.Interfaces
{
    public interface ICategoryRepository
    {
        Task<OperationResult<List<ProductCategory>>> GetAllCategoriesAsync();
        Task<OperationResult<ProductCategory>> GetCategoryByIdAsync(int id);
        

    }
}
