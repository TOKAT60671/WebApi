namespace WebApi.Interfaces
{
    public interface IAuthenticationService
    {
        string? GetCurrentAuthenticatedUserID();
    }
}
