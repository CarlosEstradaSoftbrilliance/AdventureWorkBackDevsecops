using AdventureWorks.Data;
using AdventureWorks.Options;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var frontendUrl = builder.Configuration["Frontend:BaseUrl"];

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins(frontendUrl!)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configuración de secretos (en Development ya carga user-secrets, aquí queda explícito)
builder.Configuration.AddUserSecrets<Program>(optional: true);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var connectionString =
    builder.Configuration.GetConnectionString("AdventureWorks")
    ?? Environment.GetEnvironmentVariable("ConnectionStrings__AdventureWorks")
    ?? throw new InvalidOperationException("Falta la cadena de conexión 'AdventureWorks'.");

builder.Services.AddDbContext<AdventureWorksContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.Configure<StoredProcedureOptions>(
    builder.Configuration.GetSection(StoredProcedureOptions.SectionName));

builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseCors("AllowAngular");

app.UseAuthorization();
app.MapControllers();

app.Run();

public sealed record ProductoDto(int ProductId, string Name, string ProductNumber, decimal ListPrice);
public sealed record ClienteDto(int CustomerId, string AccountNumber, int? PersonId, int? StoreId);
public sealed record OrdenVentaDto(int SalesOrderId, string SalesOrderNumber, DateTime OrderDate, byte Status, decimal TotalDue);
public sealed record EmpleadoDto(int BusinessEntityId, string JobTitle, DateOnly HireDate, bool CurrentFlag);

