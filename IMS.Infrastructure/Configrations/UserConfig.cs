using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configrations;

public class UserConfig : BaseEntityConfig<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);
        
        
        // -------------- Name -------------------
        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(30);
        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(30);
        
        //---------------- Username ----------------
        builder.Property(x => x.Username)
            .IsRequired()
            .HasMaxLength(30);
        
        builder.Property(x => x.NormalizedUserName)
            .IsRequired()
            .HasMaxLength(30);

        builder.HasIndex(x => x.NormalizedUserName).IsUnique();
        //---------------- Email ----------------
        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(150);
        
        builder.Property(x => x.NormalizedEmail)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasIndex(x => x.NormalizedEmail).IsUnique();
        
        //---------------- Password Hash ----------------
        builder.Property(x => x.PasswordHash)
            .IsRequired()
            .HasMaxLength(256)
            .IsUnicode(false);

        builder.Property(x => x.PasswordSalt)
            .IsRequired()
            .HasMaxLength(128)
            .IsUnicode(false);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);
    }
}