using HMI.Models;

namespace HMI.Services
{
    public interface IAuthenticationService
    {
        Task<AuthenticationStatus> RegisterAsync(RegisterDto register);
    }
}
