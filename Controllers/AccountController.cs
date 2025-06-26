using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;
using WebApi.Dtos;


namespace WebApi.Controllers
{
    [ApiController]
    [Route("user")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto dto)
        {
            var user = new IdentityUser { UserName = dto.UserName };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (result.Succeeded)
                return Ok();
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto dto)
        {
            var result = await _signInManager.PasswordSignInAsync(dto.UserName, dto.Password, false, false);
            if (result.Succeeded)
                return Ok();
            return Unauthorized();
        }
    }
}
/*
    public class AccountController(IAuthenticationService auth) : ControllerBase
    {
        [HttpGet(Name = "GetUserInformation")]
        [Authorize]
        public string GetUser() => auth.GetCurrentAuthenticatedUserID();
       
    }
*/
