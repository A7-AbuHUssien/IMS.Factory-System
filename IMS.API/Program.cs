using IMS.Application.Interfaces.Repositories;
using IMS.Application.Interfaces.UOW;
using IMS.Infrastructure.Persistence;
using IMS.Infrastructure.Persistence.Common;
using IMS.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using IMS.Infrastructure.Persistence.Seeding_Data;
var builder = WebApplication.CreateBuilder(args);

// ===================== Services =====================

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Repositories
// builder.Services.AddScoped<IProductRepository, ProductRepository>();
// builder.Services.AddScoped<IStockRepository, StockRepository>();
// builder.Services.AddScoped<ISalesRepository, SalesRepository>();
// builder.Services.AddScoped<IUserRepository, UserRepository>();

// Services
// builder.Services.AddScoped<IProductService, ProductService>();
// builder.Services.AddScoped<IStockService, StockService>();
// builder.Services.AddScoped<ISalesService, SalesService>();
// builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<DbInitializer>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
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
