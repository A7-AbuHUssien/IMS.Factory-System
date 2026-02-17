using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configrations;

public class RoleConfig : BaseEntityConfig<Role>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.HasIndex(x => x.Name).IsUnique();
        
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(500);
    }
}