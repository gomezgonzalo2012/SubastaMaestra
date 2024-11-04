using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SubastaMaestra.Data.Interfaces;

namespace SubastaMaestra.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ISaleRepository _saleRepository;
        public SaleController(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        [HttpGet("/list")]
        public async Task<ActionResult> GetAllSales()
        {
            var result = await _saleRepository.GetAllSalesAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("Error al encontrar ventas");
        }
        [HttpGet("/list/{id:int}")]
        public async Task<ActionResult> GetAllSalesByAuction(int id)
        {
            var result = await _saleRepository.GetAllSalesAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("Error al encontrar ventas");
        }

    }
}
