using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configrations;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(100);

        b.HasIndex(u => u.Email)
            .IsUnique();

        b.Property(u => u.PasswordHash)
            .IsRequired();
    }
}
