using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SubastaMaestra.Data.Interfaces;
using SubastaMaestra.Entities.Core;
using SubastaMaestra.Models.DTOs.Auction;

namespace SubastaMaestra.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionRepository _auctionRepository;

        public AuctionController(IAuctionRepository auctionRepository)
        {
            _auctionRepository = auctionRepository;
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateAuctionAsync([FromBody] AuctionCreateDTO auctionDTO)
        {
            if (!ModelState.IsValid) {
                return BadRequest("Modelo invalido: "+ModelState);
            }
            if (auctionDTO.StartDate < DateTime.Now) // validacion de fecha de inicio < fecha actual
            {
                return BadRequest(new
                {
                    Message = "La fecha de inicio no puede ser anterior a la fecha actual.",
                    ErrorCode = "InvalidStartDate"
                });
            }
            if (auctionDTO.FinishDate <= auctionDTO.StartDate) // validacion fecha fin > fecha inicio
            {
                return BadRequest(new
                {
                    Message = "La fecha de fin debe ser posterior a la fecha de inicio.",
                    ErrorCode = "InvalidStartDate"
                });
            }
            if (auctionDTO.StartDate.Date < DateTime.Now.AddDays(3))
            {
                return BadRequest(new
                {
                    Message = "La subasta debe terner almenos 3 días de antelación para inciar.",
                    ErrorCode = "InvalidStartDate"
                });
            }
            var result = await _auctionRepository.CreateAuctionAsync(auctionDTO);
            if (result.Success)
            {
                return Ok(result.Message);

            }
            return BadRequest("No se pudo crear la subasta");

        }

        [HttpGet("listOpen")]
        public async Task<ActionResult> GetAllOpenAuctions()
        {
            var result=   await _auctionRepository.GetAllOpenAuctionAsync();
            if (result.Success)
            {
                return Ok(result);

            }
            return BadRequest(result);
        }
        [HttpGet("list")]
        public async Task<ActionResult> GetAllAuctionAsync()
        {
            var result = await _auctionRepository.GetAllAuctionsAsync();
            return Ok(result);

        }
        [HttpGet("{id:int}")]

        public async Task<IActionResult> GetAuctionById(int id)
        {
            var result=  await _auctionRepository.GetAuctionByIdAsync(id);
            if (result.Success)
            {
                return Ok(result);

            }
            return BadRequest(result);
        }

    }
}
