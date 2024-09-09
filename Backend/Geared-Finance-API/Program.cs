using Geared_Finance_API;

var builder = WebApplication.CreateBuilder(args);
var key = builder.Configuration.GetValue<string>("ApiSettings:Secret").ToString();

builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureCors();
builder.Services.ConfigureAppsettingModel(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureAutoMapper();
builder.Services.ConfigureRepositories();
builder.Services.ConfigureJWTToken(key);
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
