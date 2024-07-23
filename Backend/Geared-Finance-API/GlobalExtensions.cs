using Entities.DBContext;
using Microsoft.EntityFrameworkCore;
using Repository.Implementation;
using Repository.Interface;
using Repository.Repos;
using Service.Implementation;
using Service.Interface;
using Service.Services;



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
                    policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
                });
            });
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
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IBaseService<>), typeof(BaseService<>));
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IVendorService,VendorService>();
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }
    }
}
