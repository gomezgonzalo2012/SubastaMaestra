using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SubastaMaestra.Data.Interfaces;
using SubastaMaestra.Entities.Core;
using SubastaMaestra.Entities.Enums;
using SubastaMaestra.Models.DTOs.Auction;
using SubastaMaestra.Models.DTOs.Reports;
using SubastaMaestra.Models.Utils;

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
            var result = await _auctionRepository.CreateAuctionAsync(auctionDTO);
            if (result.Success)
            {
                return Ok(result);

            }
            return BadRequest(
                new
                {
                    Message = result,
                    ErrorCode = "InvalidtDate"
                }
            );

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
        [HttpGet("listByState/{estado}")]
        public async Task<ActionResult> GetAllAuctionsByCurrentState(string estado)
        {
            var state = Enum.Parse< AuctionState>(estado, true);
            var result = await _auctionRepository.GetAllAuctionByCurrentStateAsync(state);
            if (result.Success )
            {
                return Ok(result);

            }else if (result.Value.Count == 0)
            {
                return NotFound(result);

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

        [HttpPatch("edit/{id:int}")]
        public async Task<IActionResult> UpdateAuction(AuctionUpdateDTO auction, int id)
        {
            var result = await _auctionRepository.EditAuctionAsync(auction, id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPatch("activar/{id:int}")]
        public async Task<IActionResult> ActivateAuction( int id)
        {
            var result = await _auctionRepository.ActivateAuctionAsync( id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPatch("deshabilitar/{id:int}")]
        public async Task<IActionResult> DisableAuction(int id)
        {
            var result = await _auctionRepository.DisableAuctionAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("masPopulares")]
        public async Task<IActionResult> GetMostPopularAucions(DateTime start, DateTime finish)
        {
            var result = await _auctionRepository.ObtenerSubastasMasPopulares(start, finish);
            if (result!= null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("reporteGanancias")]
        public async Task<IActionResult> GetProfitReport()
        {
            var result = await _auctionRepository.GetProfitReport();
            if (result != null)
            {
                return Ok(result);
            }
            return NoContent();
        }
    }
}
