// Tus usings habituales. Asegúrate de tener este para las personalizaciones de OpenAPI.
using Microsoft.OpenApi.Models; // Necesario para OpenApiInfo

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(); // Esto es para tus controladores tradicionales (como WeatherForecastController)

// ********** SERVICIOS DE OPENAPI/SWAGGER (con Microsoft.AspNetCore.OpenApi) **********
// La plantilla de .NET 8 con Web API tradicional o Minimal APIs generalmente incluye esto:
builder.Services.AddEndpointsApiExplorer(); // Permite descubrir los endpoints
builder.Services.AddSwaggerGen(options => // Aquí puedes personalizar la info de tu API
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EcoTrip API",
        Version = "v1",
        Description = "API para la gestión de usuarios, viajes y destinos de EcoTrip."
    });
});
// ************************************************************************************

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // ********** MIDDLEWARE DE OPENAPI/SWAGGER (con Microsoft.AspNetCore.OpenApi) **********
    app.UseSwagger(); // Este es correcto, habilita el middleware para servir el JSON de OpenAPI
    app.UseSwaggerUI(options => // Este también es correcto, habilita la interfaz de Swagger UI
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "EcoTrip API v1");
        options.RoutePrefix = string.Empty; // Para acceder en la raíz (http://localhost:PUERTO/)
    });
    // *************************************************************************************
}

app.UseHttpsRedirection();

// Asegúrate de que cualquier configuración de CORS esté aquí antes de UseAuthorization
// app.UseCors("MyAllowSpecificOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();