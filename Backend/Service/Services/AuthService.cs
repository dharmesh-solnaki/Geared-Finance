using Entities.DTOs;
using Entities.Models;
using Entities.UtilityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;
using Service.Implementation;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
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
            BaseSearchEntity<User> baseSearchEntity = new BaseSearchEntity<User>()
            {
                predicate = x => (x.Email == model.Email && x.Password == model.Password),
                includes = new Expression<Func<User, object>>[] { x => x.Role }
            };
            IQueryable<User> userData = await _userRepo.GetAllAsync(baseSearchEntity);
            if (!userData.Any())
            {
                return null;
            }
            User user = await userData.FirstOrDefaultAsync();

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
                    new Claim(JWTTokenClaims.USER_ROLE, user.Role.RoleName),
                    //new Claim(JWTTokenClaims.ROLE_ID,user.RoleId.ToString()),
                    new Claim(JWTTokenClaims.USER_NAME,user.Name),
                    new Claim(JWTTokenClaims.USER_ID,user.Id.ToString()),
                    new Claim(JWTTokenClaims.REF_TOKEN_EXP_TIME,refreshTokenExpTime.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(double.Parse(jwtSettings["AccessTokenExpTime"])),
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
                ValidateLifetime = false,
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
            var newExpTime = DateTime.UtcNow.AddHours(double.Parse(_configuration["ApiSettings:AccessTokenExpTime"]));
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
            BaseSearchEntity<User> baseSearchEntity = new BaseSearchEntity<User>()
            {
                predicate = x => x.Email == email
            };
            IQueryable<User> userData = await _userRepo.GetAllAsync(baseSearchEntity);
            if (!userData.Any())
            {
                return false;
            }
            User user = await userData.FirstOrDefaultAsync();
            await GenerateMailAsync(user);
            return true;
        }

        private async Task GenerateMailAsync(User user)
        {
            string otp = StringGenerator.GenerateOtp(6, false);
            user.Otp = otp;
            await _userRepo.UpdateAsync(user);
            string mailTo = "dharmesh.solanki@tatvasoft.com";
            string subject = "About Forgot Password";
            string body = $" This is otp to set your new password: \n {otp}";
            await MailSenderAsync.SendMailAsync(mailTo, subject, body);
        }


        public async Task<bool> ValidateOtpAsync(OtpRequest model)
        {
            BaseSearchEntity<User> baseSearchEntity = new BaseSearchEntity<User>()
            {
                predicate = x => x.Email == model.Email
            };
            IQueryable<User> userData = await _userRepo.GetAllAsync(baseSearchEntity);
            if (!userData.Any())
            {
                return false;
            }
            User user = await userData.FirstOrDefaultAsync();
            return user.Otp == model.Otp;
        }

        public async Task<bool> UpdatePasswordAsync(PasswordUpdateReq model)
        {
            BaseSearchEntity<User> baseSearchEntity = new BaseSearchEntity<User>()
            {
                predicate = x => x.Email == model.Email
            };
            IQueryable<User> userData = await _userRepo.GetAllAsync(baseSearchEntity);
            if (!userData.Any())
            {
                return false;
            }
            User user = await userData.FirstOrDefaultAsync();
            user.Password = SecretHasher.EnryptString(model.Password);
            await _userRepo.UpdateAsync(user);
            return true;
        }
    }
}
