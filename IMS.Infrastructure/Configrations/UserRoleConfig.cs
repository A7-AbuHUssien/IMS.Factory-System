using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configrations;

public class UserRoleConfig : BaseEntityConfig<UserRole>
{
    public override void Configure(EntityTypeBuilder<UserRole> b)
    {
        base.Configure(b);

        b.HasIndex(ur => new { ur.UserId, ur.RoleId })
            .IsUnique();

        b.HasIndex(ur => ur.RoleId);

        b.HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
