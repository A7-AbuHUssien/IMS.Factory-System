using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configrations;

public class UserConfig : BaseEntityConfig<User>
{
    public override void Configure(EntityTypeBuilder<User> b)
    {
        base.Configure(b);
        
        b.Property(u=>u.Name)
            .IsRequired()
            .HasMaxLength(50);
        
        b.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(100);

        b.HasIndex(u => u.Email)
            .IsUnique();

        b.Property(u => u.PasswordHash)
            .IsRequired();

        b.Property(u => u.IsActive)
            .HasDefaultValue(true);
        
        b.HasMany(u => u.UserRoles)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasMany(u => u.StockTransactions)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
