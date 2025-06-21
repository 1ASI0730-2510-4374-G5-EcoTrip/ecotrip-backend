using Microsoft.OpenApi.Models;
using ecotrip_backend.Auth.Application.Services;
using ecotrip_backend.Auth.Application.Services.Interfaces;
using ecotrip_backend.Auth.Domain.Repositories.Interfaces;
using ecotrip_backend.Auth.Infrastructure.Repositories;
using ecotrip_backend.Auth.Infrastructure.Persistence;
using ecotrip_backend.Tourists.Domain;
using ecotrip_backend.Tourists.Infrastructure.EF;
using ecotrip_backend.Tourists.Infrastructure.Persistence;
using ecotrip_backend.Tourists.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

// Database setup
if (builder.Environment.IsDevelopment())
{
    // Use in-memory repository for development
    builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
    
    // For demonstration purposes, use in-memory DB for Tourist as well
    builder.Services.AddDbContext<EcoTripDbContext>(options =>
        options.UseInMemoryDatabase("TouristDb"));
}
else
{
    // Use SQL Server for production
    builder.Services.AddDbContext<AuthDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    
    // Configure Tourist DB for production
    builder.Services.AddDbContext<EcoTripDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITouristRepository, TouristRepository>();
builder.Services.AddScoped<TouristService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EcoTrip API",
        Version = "v1",
        Description = "API para la gestión de usuarios, viajes y destinos de EcoTrip."
    });
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => 
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "EcoTrip API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();