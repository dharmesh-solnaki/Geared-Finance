using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;

namespace Geared_Finance_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> GetAuthenticate(LoginDTO model)
        {
            var accessToken = await _authService.GenerateToken(model);
            return Ok(new { accessToken });
        }

        [HttpGet("validateToken")]
        public async Task<IActionResult> ValidateToken([FromHeader(Name = "Authorization")] string token)
        {
            token = token.Substring("Bearer ".Length).Trim();
            var accessToken = await _authService.ValidateRefreshToken(token);
            return Ok(new { accessToken });
        }

        [HttpGet]
        public async Task<IActionResult> ForgotPass([FromQuery] string email)
        {
            bool isMailExist = await _authService.IsValidMailAsync(email);
            return Ok(new { isMailExist });
        }

        [HttpPost("validateOtp")]
        public async Task<IActionResult> ValidateOtp(OtpRequest model)
        {
            bool isValidOtp = await _authService.ValidateOtpAsync(model);
            return Ok(new { isValidOtp });
        }
        [HttpPost("updateCredential")]

        public async Task<IActionResult> UpdatePassword(PasswordUpdateReq model)
        {
            bool isPassUpdated = await _authService.UpdatePasswordAsync(model);
            return Ok(new { isPassUpdated });
        }

    }
}
