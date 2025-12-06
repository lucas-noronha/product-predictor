using Microsoft.EntityFrameworkCore;
using ProductPrediction.API.AIService.Interfaces;
using ProductPrediction.API.Infrastructure;
using ProductPrediction.API.Services;

var builder = WebApplication.CreateBuilder(args);

// registra o DbContext com a connection string "Default"
var connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorCodesToAdd: null);
    }));

// Swagger + Controllers
builder.Services.AddControllers(); // <- IMPORTANTE!
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Serviço de IA
builder.Services.AddScoped<IPurchasePredictionService, PurchasePredictionService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


var app = builder.Build();

app.UseCors("AllowAll");
// ================================
// MIGRATIONS AUTOMÁTICAS (idempotente)
// ================================
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
        Console.WriteLine("Migrations aplicadas com sucesso.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao aplicar migrations: {ex.Message}");
    }
}

// Swagger e endpoints
app.UseSwagger();
app.UseSwaggerUI();

// pipeline HTTP
app.UseAuthorization();

// Controllers mapeados
app.MapControllers(); // <- IMPORTANTE!

// endpoint simples
app.MapGet("/", () => "API ok");

app.Run();
