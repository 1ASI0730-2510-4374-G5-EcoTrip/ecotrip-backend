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