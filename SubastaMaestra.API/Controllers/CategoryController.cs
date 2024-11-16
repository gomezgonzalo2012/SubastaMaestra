using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SubastaMaestra.Data.Interfaces;

namespace SubastaMaestra.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository) {
            this._categoryRepository = categoryRepository;
        
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllCategories() {
            var result = await _categoryRepository.GetAllCategoriesAsync();
            if (result.Success == true && result.Value!=null)
            {
                return Ok(result);

            }
            else if (result.Success == true && result.Value == null)
            {
                return NotFound(result);

            }
            return BadRequest(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAllCategories(int id)
        {
            var result = await _categoryRepository.GetCategoryByIdAsync(id);
            if (result.Success == true && result.Value != null)
            {
                return Ok(result);

            }else if (result.Success == true && result.Value == null)
            {
                return NotFound(result);

            }
            return BadRequest(result);
        }
    }
}
