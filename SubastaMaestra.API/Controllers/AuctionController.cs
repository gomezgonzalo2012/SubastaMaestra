using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SubastaMaestra.Data.Interfaces;
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
                return BadRequest("Modelo invalido"+ModelState);
            }
            
            var result = await _auctionRepository.CreateAuctionAsync(auctionDTO);
            if (result == 1)
            {
                return Ok();

            }
            return BadRequest("No se pudo crear la subasta");

        }

        [HttpGet("open-auctions")]
        public async Task<ActionResult<List<AuctionDTO>>> GetAllOpenAuctions()
        {
            return  await _auctionRepository.GetAllOpenAuctionAsync();   
        }

        [HttpGet("{id:int}")]

        public async Task<AuctionDTO> GetAuctionById(int id)
        {
            return await _auctionRepository.GetAuctionByIdAsync(id);
        }

    }
}
