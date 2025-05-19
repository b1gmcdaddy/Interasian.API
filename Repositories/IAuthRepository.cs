using Interasian.API.DTOs;
using Interasian.API.Models;

namespace Interasian.API.Repositories
{
    public interface IAuthRepository
    {
        Task<(bool succeeded, string message)> RegisterUserAsync(RegisterDTO model);
        Task<(bool succeeded, string? token)> LoginUserAsync(LoginDTO model);
        Task<string> GenerateJwtTokenAsync(User user);
    }
} 