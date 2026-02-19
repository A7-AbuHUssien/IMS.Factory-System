using System.Configuration;
using System.Text;
using IMS.API.Middlewares;
using IMS.Application;
using IMS.Infrastructure;
using IMS.Infrastructure.Persistence;
using IMS.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));
builder.Services.AddAutoMapper(typeof(Program));

// ================= Dependency Injection =====================
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

// ===================== Controllers =====================
builder.Services.AddControllers();

// ===================== Swagger =====================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
// ===================== JWT-CHECK =====================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtIssuer"],
            ValidAudience = builder.Configuration["JwtAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// ===================== Build =====================
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

app.Run();