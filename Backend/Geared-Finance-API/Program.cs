using Geared_Finance_API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddDbContext<ApplicationDBContext>(options =>
//{
//    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
//});
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("", policy =>
//    {
//        policy.WithOrigins("http://localhost:4200").AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
//    });
//});
//builder.Services.AddAutoMapper(typeof(MappingConfig));
//builder.Services.AddScoped(typeof(IBaseRepo<>),typeof(BaseRepo<>));
//builder.Services.AddTransient(typeof(IBaseService<>), typeof(BaseService<>));
//builder.Services.AddScoped<IUserRepo,UserRepo>();
//builder.Services.AddTransient<IUserService, UserService>();
//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureCors();
builder.Services.ConfigureAutoMapper();
builder.Services.ConfigureRepositories();
builder.Services.ConfigureServices();
builder.Services.ConfigureSwagger();
builder.Services.AddControllers();

var app = builder.Build();
app.UseMiddleware<ExceptionHandler>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
