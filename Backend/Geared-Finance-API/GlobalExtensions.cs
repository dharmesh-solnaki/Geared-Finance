using Entities.DBContext;
using Entities.UtilityModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository.Implementation;
using Repository.Interface;
using Repository.Repos;
using Service.Implementation;
using Service.Interface;
using Service.Services;
using System.Text;
using Utilities;



namespace Geared_Finance_API
{
    public static class GlobalExtensions
    {
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDBContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Default"));
            });

        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.WithOrigins("http://localhost:58743").AllowAnyHeader().AllowAnyMethod();
                });
            });
        }

        public static void ConfigureAppsettingModel(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailCredentials"));
            services.Configure<ApiSettings>(configuration.GetSection("ApiSettings"));
        }

        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingConfig));

        }

        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepo<>), typeof(BaseRepo<>));
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IVendorRepo, VendorRepo>();
            services.AddScoped<IEquipmentRepo, EquipmentRepo>();
            services.AddScoped<IRolePermissionRepo, RolePermissionRepo>();
            services.AddScoped<IFunderRepo, FunderRepo>();

        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IBaseService<>), typeof(BaseService<>));
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IVendorService, VendorService>();
            services.AddTransient<IEquipmentService, EquipmentService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IRolePermissionService, RolePermissionService>();
            services.AddTransient<IFunderService, FunderService>();
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(
              options =>
              {
                  options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                  {
                      Name = "Authentication",
                      In = ParameterLocation.Header,
                      Type = SecuritySchemeType.Http,
                      Scheme = "Bearer"
                  });
                  options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                   {
                      {
                         new  OpenApiSecurityScheme
                         {
                            Reference = new OpenApiReference {
                                 Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                            Scheme = "oauth2"
                         },
                          new List<string> ()
                      }
                   });
              });
        }

        public static void ConfigureJWTToken(this IServiceCollection services, string key)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = true
,
                };

            });
        }
    }
}
