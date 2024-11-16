using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using SubastaMaestra.Data.Interfaces;
using SubastaMaestra.Models.DTOs.User;
using SubastaMaestra.Models.Utils;
using SubastaMaestra.Entities.Core;

namespace SubastaMaestra.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IProductRepository _productRepository;
        private readonly IBidRepository _bidRepository;
        public UserController(IUserRepository userRepository, INotificationRepository notifRepository,
            IProductRepository productRepository, IBidRepository bidRepository) { 
            _userRepository = userRepository;
            _notificationRepository = notifRepository;
            _productRepository = productRepository;
            _bidRepository = bidRepository;
        }
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UserCreateDTO userCreateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Modelo Invalido" + ModelState);
            }
            var result = await _userRepository.RegisterUserAsync(userCreateDTO);
            if (result.Success==false)
            {
                return BadRequest(result);
            }
            else 
            {
                return Ok(result);
            }
        }
        [HttpGet("/{userId:int}/notifications")]
        public async Task<ActionResult> GetUserNotifications(int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Modelo invalido" + ModelState);
            }
            var result = await _notificationRepository.GetAllNotificationsByUserAsync(userId);

            if (result.Success == false) {
                return BadRequest(result);
            }
            return Ok(result);

        }

        [HttpGet("masActivos")]
        public async Task<IActionResult> GetMostActiveUsers()
        {
            var result = await _userRepository.ObtenerUsuariosMasActivos();
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("misProductos/{userId:int}")]
        public async Task<IActionResult> GetProductsBySeller(int userId)
        {
            var products = await _productRepository.GetProductsBySeller(userId);
            if (products.Success && products.Value.Count() > 0)
            {
                return Ok(products);
            }
            else if (products.Success && products.Value.Count() == 0 )
            {
                return Ok(products);
            }
            return BadRequest(products);
        }

        [HttpGet("misOfertas/{userId:int}")]
        public async Task<IActionResult> GetBidsByUser(int userId)
        {
            var bids = await _bidRepository.GetBidsByUser(userId);
            if (bids.Success && bids.Value.Count() > 0)
            {
                return Ok(bids);
            }
            else if (bids.Success && bids.Value.Count() == 0)
            {
                return Ok(bids);
            }
            return BadRequest(bids);
        }

        //[HttpGet("validate")]
        //public async Task<ActionResult> ValidateUser(string email, string password)
        //{
        //    ////var result = await _userRepository.ValidateUserAsync(email, password);
        //    //if (result.Success == true)
        //    //{
        //    //    return Ok(result);
        //    //}
        //    //return BadRequest(result );
        //}
    }
}
