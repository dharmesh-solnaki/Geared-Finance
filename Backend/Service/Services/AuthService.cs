using Entities.DTOs;
using Entities.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;
using Service.Implementation;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using System.Text;
using Utilities;

namespace Service.Services
{
    public class AuthService : BaseService<User>, IAuthService
    {
        private readonly IBaseRepo<User> _userRepo;
        private readonly string secretKey;
        private readonly IConfiguration _configuration;
        public AuthService(IBaseRepo<User> repo, IConfiguration configuration) : base(repo)
        {
            _userRepo = repo;
            _configuration = configuration;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public async Task<string> GenerateToken(LoginDTO model)
        {
            model.Password = SecretHasher.EnryptString(model.Password.Trim());
            Expression<Func<User, bool>> predicate = x => (x.Email == model.Email && x.Password == model.Password);
            var includes = new Expression<Func<User, object>>[]
             {
                x=>x.Role
             };
            User user = await _userRepo.GetByOtherAsync(predicate, includes);
            if (user == null)
            {
                return null;
            }
            var jwtSettings = _configuration.GetSection("ApiSettings");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);
            var refreshTokenExpTime = DateTime.UtcNow.AddDays(double.Parse(jwtSettings["RefreshTokenExpTime"]));
            if (model.IsRemember)
            {
                refreshTokenExpTime = DateTime.UtcNow.AddDays(double.Parse(jwtSettings["RefreshTokenExpTimeOnRemember"]));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Role , user.Role.RoleName),
                    new Claim("userName",user.Name),
                    new Claim("userId",user.Id.ToString()),
                    new Claim("refreshTokenExp",refreshTokenExpTime.ToString("o"))
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["AccessTokenExpTime"])),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));


            return token;
        }

      

        public async Task<string> ValidateRefreshToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            if (!token.Any())
            {
                return null;
            }
            var refreshTokenExpClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "refreshTokenExp")?.Value;
            if (string.IsNullOrEmpty(refreshTokenExpClaim) || !DateTime.TryParse(refreshTokenExpClaim, out var refreshTokenExp))
            {
                return null;
            }
            if (refreshTokenExp < DateTime.UtcNow)
            {
                return null;
            }

            var validationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["ApiSettings:Secret"]))
            };
            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            }
            catch
            {
                return null;
            }
            var key = Encoding.ASCII.GetBytes(_configuration["ApiSettings:Secret"]);
            var newExpTime = DateTime.UtcNow.AddMinutes(1);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(jwtToken.Claims),
                Expires = newExpTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var newToken = tokenHandler.CreateToken(tokenDescriptor);
            var newJwtToken = tokenHandler.WriteToken(newToken);

            return newJwtToken;

        }

        public async Task<bool> IsValidMailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            Expression<Func<User,bool>> predicate = x=>x.Email == email;
            User user = await _userRepo.GetByOtherAsync(predicate,null);
            if (user == null) return false;
            string otp = StringGenerator.GenerateOtp(6,false);
            user.Otp = otp;
            await _userRepo.UpdateAsync(user);
            string mailTo = "gfivhcrte@emltmp.com";
            string subject = "About Forgot Password";
            string body=$" This is otp to set your new password: \n {otp}" ;
            await new MailSenderAsync().SendMailAsync(mailTo,subject,body);
            return true;
        }

        public async Task<bool> ValidateOtpAsync(OtpRequest model)
        {
            Expression<Func<User,bool>> predicate = x=>x.Email==model.Email;
            User user = await _userRepo.GetByOtherAsync(predicate, null);
            if (user == null) return false;
            return user.Otp == model.Otp;
        }

        public async Task<bool> UpdatePasswordAsync(PasswordUpdateReq model)
        {
            Expression<Func<User, bool>> predicate = x => x.Email == model.Email;
            User user = await _userRepo.GetByOtherAsync(predicate, null);
            if (user == null) return false;
            user.Password = SecretHasher.EnryptString(model.Password);
            await _userRepo.UpdateAsync(user);
            return true;
        }
    }
}
