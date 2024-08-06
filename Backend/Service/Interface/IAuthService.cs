using Entities.DTOs;
using Entities.Models;
using Service.Interface;

namespace Repository.Interface
{
    public interface IAuthService : IBaseService<User>
    {
        Task<string> GenerateToken(LoginDTO model);
        Task<bool> IsValidMailAsync(string email);
        Task<bool> UpdatePasswordAsync(PasswordUpdateReq model);
        Task<bool> ValidateOtpAsync(OtpRequest model);
        Task<string> ValidateRefreshToken(string token);
    }
}
