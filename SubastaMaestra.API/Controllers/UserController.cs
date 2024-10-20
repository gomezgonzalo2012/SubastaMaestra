using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using SubastaMaestra.Data.Interfaces;
using SubastaMaestra.Models.DTOs.User;
using SubastaMaestra.Models.Utils;

namespace SubastaMaestra.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository) { 
            _userRepository = userRepository;
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
