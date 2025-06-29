using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;
using WebApi.Dtos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace WebApi.Controllers
{
    [ApiController]
    [Route("user")]
    public class AccountController : ControllerBase
    {
        private readonly IDatabaseRepository _repo;
        private readonly IConfiguration _config;

        public AccountController(IDatabaseRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto dto)
        {
            var existing = await _repo.GetUserByUserName(dto.UserName);
            if (existing != null)
                return BadRequest("User already exists.");

            await _repo.InsertUser(dto);
            return Ok("Registration successful.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto dto)
        {
            if (string.IsNullOrEmpty(dto?.UserName) || string.IsNullOrEmpty(dto?.Password))
                return BadRequest("Username and password are required.");


            var user = await _repo.GetUserByUserName(dto.UserName);
            if (user == null || user.Password != dto.Password)
                return Unauthorized("Invalid username or password.");
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { accessToken });
        }
    }
}