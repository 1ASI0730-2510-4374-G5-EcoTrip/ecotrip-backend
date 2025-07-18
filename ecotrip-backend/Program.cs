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
using Experience.Domain.Repositories;
using Experience.Infrastructure.Persistence.Repositories;
using Experience.Infrastructure.Persistence;
using Experience.Application.Commands.CreateExperience;
using Experience.Application.Commands.DeleteExperience;
using Experience.Application.Commands.UpdateExperience;
using Experience.Application.Queries.GetExperience;
using Experience.Application.Queries.ListExperience;
using Experience.Application.Services;
using Experience.Infrastructure.Services.ImageStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel for Render deployment
builder.WebHost.ConfigureKestrel(options =>
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
    options.ListenAnyIP(int.Parse(port));
});

builder.Services.AddControllers();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
    builder.Services.AddDbContext<EcoTripDbContext>(options =>
        options.UseInMemoryDatabase("TouristDb"));
    builder.Services.AddDbContext<ExperienceDbContext>(options =>
        options.UseInMemoryDatabase("ExperienceDb"));
}
else
{
    builder.Services.AddDbContext<AuthDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    
    builder.Services.AddDbContext<EcoTripDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    
    builder.Services.AddDbContext<ExperienceDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITouristRepository, TouristRepository>();
builder.Services.AddScoped<TouristService>();


builder.Services.AddScoped<IExperienceRepository, ExperienceRepository>();
builder.Services.AddScoped<CreateExperienceCommandHandler>();
builder.Services.AddScoped<DeleteExperienceCommandHandler>();
builder.Services.AddScoped<UpdateExperienceCommandHandler>();
builder.Services.AddScoped<GetExperienceQueryHandler>();
builder.Services.AddScoped<ListExperiencesQueryHandler>();
builder.Services.AddScoped<ImageUploadService>();
builder.Services.AddScoped<LocalFileImageStorage>();

// Reservations services
builder.Services.AddScoped<ecotrip_backend.Reservations.Domain.Repositories.IBookingRepository, ecotrip_backend.Reservations.Infrastructure.Persistence.BookingRepository>();
builder.Services.AddScoped<ecotrip_backend.Reservations.Application.Services.BookingService>();

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
    
    // Include XML comments
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Enable Swagger in all environments for Render deployment
app.UseSwagger();
app.UseSwaggerUI(options => 
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "EcoTrip API v1");
    options.RoutePrefix = "swagger";
});

// Configure the HTTP request pipeline
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }
    await next();
});

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();