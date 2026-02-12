using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configrations;

public class RoleConfig : BaseEntityConfig<Role>
{
    public override void Configure(EntityTypeBuilder<Role> b)
    {
        base.Configure(b);
        b.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(50);

        b.HasIndex(r => r.Name)
            .IsUnique();

        b.HasMany(r => r.UserRoles)
            .WithOne(ur => ur.Role)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
