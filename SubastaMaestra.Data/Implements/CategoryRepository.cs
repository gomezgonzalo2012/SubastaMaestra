using Microsoft.EntityFrameworkCore;
using SubastaMaestra.Data.Interfaces;
using SubastaMaestra.Entities.Core;
using SubastaMaestra.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Data.Implements
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly SubastaContext _subastaContext;
        public CategoryRepository(SubastaContext context) {     
        this._subastaContext = context;
        }
        public async Task<OperationResult<List<ProductCategory>>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _subastaContext.ProductCategories.ToListAsync();
                if (!categories.Any())
                {
                    return new OperationResult<List<ProductCategory>> { Success = true, Message="Sin categorias.",Value = categories };

                }
                return new OperationResult<List<ProductCategory>> { Success = true, Value= categories };
            }
            catch (Exception ex)
            {
                return new OperationResult<List<ProductCategory>> { Success = false, Message = ex.Message, Value = null };
            }
        }

        public async Task<OperationResult<ProductCategory>> GetCategoryByIdAsync(int id)
        {
            try
            {
                var category = new ProductCategory();
                 category = await _subastaContext.ProductCategories.Where(c=>c.Id == id).FirstOrDefaultAsync();
                if(category == null)
                {
                    return new OperationResult<ProductCategory> { Success = true, Message = "Categoria no encontrada.", Value = category };

                }
                return new OperationResult<ProductCategory> { Success = true, Value = category };
            }
            catch (Exception ex)
            {
                return new OperationResult<ProductCategory> { Success = false, Message = ex.Message, Value = null };
            }
        }
    }
}
