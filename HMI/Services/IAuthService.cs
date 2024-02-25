using HMI.Models;

namespace HMI.Services
{
    public interface IAuthService
    {
        Task<AuthStatus> RegisterAsync(RegisterDto register);
        Task<AuthStatus> LoginAsync(LoginDto loging);
        Task LogoutAsync();
    }
}
