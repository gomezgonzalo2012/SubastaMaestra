using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SubastaMaestra.Data.Interfaces;
using SubastaMaestra.Models.DTOs.User;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using SubastaMaestra.Entities.Core;

namespace SubastaMaestra.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly  IUserRepository _userRepository;
        IConfiguration _config;

        public LoginController( IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDTO userDTO)
        {
            var result = await _userRepository.ValidateUserAsync(userDTO);
            var user = result.Value;
            if (user is null)
            {
                return BadRequest(new {message = "credenciales invalidas"});

            }
            string jwtToken = GenerateToken(user);
            // generar un token 
            return Ok(new { token = jwtToken });

        }

        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                 new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JWT:Key").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken( // genera token 
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds
                    );

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken); // se serializa en string 
            return token;
        }
    }
}
