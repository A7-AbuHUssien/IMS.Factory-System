using IMS.API.Middlewares;
using IMS.Application;
using IMS.Infrastructure;
using IMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using IMS.Infrastructure.Persistence.Seeding_Data;
var builder = WebApplication.CreateBuilder(args);

// ===================== Services =====================

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddScoped<DbInitializer>();

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// ===================== Pipeline =====================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// ===================== Database Initialization =====================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var initializer = services.GetRequiredService<DbInitializer>();
        initializer.Initialize();
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "ERROR INITIALIZING DATABASE (Migrations/Seed)");
    }
}

app.Run();
