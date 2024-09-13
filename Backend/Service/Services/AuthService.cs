using Entities.DTOs;
using Entities.Models;
using Entities.UtilityModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;
using Service.Implementation;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using Utilities;

namespace Service.Services;

public class AuthService : BaseService<User>, IAuthService
{
    private readonly IBaseRepo<User> _userRepo;
    private readonly IOptions<EmailSettings> _emailSettings;
    private readonly ApiSettings _apiSettings;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(IBaseRepo<User> repo, IOptions<EmailSettings> settings, IOptions<ApiSettings> apiSettings, IHttpContextAccessor httpContextAccessor) : base(repo)
    {
        _userRepo = repo;
        _emailSettings = settings;
        _apiSettings = apiSettings.Value;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> GenerateToken(LoginDTO model)
    {
        model.Password = SecretHasher.EnryptString(model.Password.Trim());
        BaseSearchEntity<User> baseSearchEntity = new()
        {
            predicate = x => (x.Email == model.Email && x.Password == model.Password),
            includes = new Expression<Func<User, object>>[] { x => x.Role }
        };
        IQueryable<User> userData = await _userRepo.GetAllAsync(baseSearchEntity);
        User user = await userData.FirstOrDefaultAsync() ?? throw new KeyNotFoundException(Constants.RECORD_NOT_FOUND);
        var tokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.ASCII.GetBytes(_apiSettings.Secret);
        DateTime refreshTokenExpTime = DateTime.UtcNow.AddDays(_apiSettings.RefreshTokenExpTime);
        if (model.IsRemember)
        {
            refreshTokenExpTime = DateTime.UtcNow.AddDays(_apiSettings.RefreshTokenExpTimeOnRemember);
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(Constants.USER_ROLE, user.Role.RoleName),
                new(Constants.USER_NAME,user.Name),
                new(Constants.USER_ID,user.Id.ToString()),
                new(Constants.REF_TOKEN_EXP_TIME,refreshTokenExpTime.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(_apiSettings.AccessTokenExpTime),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
    }



    public async Task<string> ValidateRefreshToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
        var refreshTokenExpClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == Constants.REF_TOKEN_EXP_TIME)?.Value;
        if (string.IsNullOrEmpty(refreshTokenExpClaim) || !DateTime.TryParse(refreshTokenExpClaim, out var refreshTokenExp))
        {
            throw new KeyNotFoundException(Constants.INVALID_TOKEN);

        }
        if (refreshTokenExp < DateTime.UtcNow)
        {
            throw new KeyNotFoundException(Constants.TOKEN_EXPIRED);
        }

        var validationParameters = new TokenValidationParameters
        {
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_apiSettings.Secret))
        };
        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
        }
        catch
        {
            throw new KeyNotFoundException(Constants.INVALID_TOKEN);
        }
        var key = Encoding.ASCII.GetBytes(_apiSettings.Secret);
        DateTime newExpTime = DateTime.UtcNow.AddHours(_apiSettings.AccessTokenExpTime);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(jwtToken.Claims),
            Expires = newExpTime,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var newToken = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(newToken);
    }

    public async Task<bool> IsValidMailAsync(string email)
    {
        BaseSearchEntity<User> baseSearchEntity = new()
        {
            predicate = x => x.Email == email
        };
        IQueryable<User> userData = await _userRepo.GetAllAsync(baseSearchEntity);
        if (!userData.Any())
        {
            return false;
        }
        User user = await userData.FirstOrDefaultAsync() ?? throw new InvalidLogicException(Constants.RECORD_NOT_FOUND); ;
        await GenerateMailAsync(user);
        return true;
    }

    private async Task GenerateMailAsync(User user)
    {
        string otp = StringGenerator.GenerateOtp(6, false);
        user.Otp = otp;
        await _userRepo.UpdateAsync(user);
        string mailTo = Constants.TO_MAIL;
        string subject = Constants.OTP_MAIL_SUBJECT;
        string body = string.Format(Constants.OTP_MAIL_BODY_TEMPLATE, otp);
        await MailSenderAsync.SendMailAsync(mailTo, subject, body, _emailSettings.Value);
    }

    public async Task<bool> ValidateOtpAsync(OtpRequest model)
    {
        BaseSearchEntity<User> baseSearchEntity = new()
        {
            predicate = x => x.Email == model.Email
        };
        IQueryable<User> userData = await _userRepo.GetAllAsync(baseSearchEntity);
        if (!userData.Any())
        {
            return false;
        }
        User? user = await userData.FirstOrDefaultAsync();
        return user != null && user.Otp == model.Otp;
    }

    public async Task<bool> UpdatePasswordAsync(PasswordUpdateReq model)
    {
      
        BaseSearchEntity<User> baseSearchEntity = new()
        {
            predicate = x => x.Email == model.Email
        };
        IQueryable<User> userData = await _userRepo.GetAllAsync(baseSearchEntity);
        if (!userData.Any())
        {
            return false;
        }
        User user = await userData.FirstOrDefaultAsync() ?? throw new KeyNotFoundException(Constants.RECORD_NOT_FOUND);
        user.Password = SecretHasher.EnryptString(model.Password);
        await _userRepo.UpdateAsync(user);
        return true;
    }

    public int GetUserId()
    {
        try
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Split(" ").Last();
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == Constants.USER_ID);
            return int.Parse(userIdClaim.Value);
        }
        catch
        {
            throw;
        }

    }
}
