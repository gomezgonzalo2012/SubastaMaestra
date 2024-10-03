using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        //[HttpPost("create")]
        //public async Task<ActionResult> CreateBid([FromBody] BidCreateDTO bidCreateDTO)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest("Modelo inválido: " + ModelState);
        //    }
            
        //}

    }
}
