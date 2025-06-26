using WebApi.Interfaces;
using System.Security.Claims;
using WebApi.Dtos;
namespace WebApi.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthenticationService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Gets the current users' ID
        /// </summary>
        /// <returns>string?</returns>
        public string? GetCurrentAuthenticatedUserID()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
