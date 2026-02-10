using IMS.Domain.Entities;
using IMS.Infrastructure.Persistence.SD;
using Microsoft.EntityFrameworkCore;

namespace IMS.Infrastructure.Persistence.Seeding_Data;

public class DbInitializer // حذف كلمة static
{
    private readonly ApplicationDbContext _context;

    public DbInitializer(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Initialize() // ميثود عادية وليس Extension
    {
        // التأكد من تنفيذ الـ Migrations
        if (_context.Database.GetPendingMigrations().Any())
            _context.Database.Migrate();

        // فحص وجود الأدوار (Roles)
        if (!_context.Roles.Any())
        {
            _context.Roles.AddRange(
                new Role { Id = StaticData.AdminRoleId, Name = StaticData.ADMIN_ROLE, CreatedAt = DateTime.UtcNow },
                new Role { Id = StaticData.ManagerRoleId, Name = StaticData.MANAGER_ROLE, CreatedAt = DateTime.UtcNow },
                new Role { Id = StaticData.EmployeeRoleId, Name = StaticData.MAIN_WAREHOUSE_ROLE, CreatedAt = DateTime.UtcNow }
            );
        }

        // فحص وجود المستخدم (User)
        if (!_context.Users.Any())
        {
            _context.Users.Add(new User 
            { 
                Id = StaticData.AdminUserId,
                Name = "Ahmed Esam",
                Email = "admin@factory.com", 
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), 
                IsActive = true,
                CreatedAt = DateTime.UtcNow 
            });
        }

        _context.SaveChanges();
    }
}