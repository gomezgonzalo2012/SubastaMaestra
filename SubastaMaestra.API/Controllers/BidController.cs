using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SubastaMaestra.Data.Implements;
using SubastaMaestra.Data.Interfaces;
using SubastaMaestra.Models.DTOs.Bid;

namespace SubastaMaestra.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private readonly IBidRepository _bidRepository;
        private readonly IMapper _mapper;

        public BidController(IBidRepository bidRepository, IMapper mapper)
        {
            _bidRepository = bidRepository;
            _mapper = mapper;
        }

        // crear un puja

        [HttpPost("create")]
        public async Task<ActionResult> CreateBid(BidCreateDTO bidCreateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Modelo inválido: " + ModelState);
            }

            var result = await _bidRepository.CreateBid(bidCreateDTO);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpGet("/product/{id:int}")]
        public async Task<ActionResult> GetBiddersByProduct(int id)
        {
            var result = await _bidRepository.GetBiddersByProduct(id);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

    }
}
