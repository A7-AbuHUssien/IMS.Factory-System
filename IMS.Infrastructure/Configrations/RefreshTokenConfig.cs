using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configrations;

public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasDefaultValueSql("NEWID()");

        builder.Property(x => x.TokenHash)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.CreatedOn)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(x => x.IsUsed)
            .HasDefaultValue(false);

        builder.Property(x => x.IsRevoked)
            .HasDefaultValue(false);

        builder.HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasQueryFilter(rft => !rft.IsUsed && !rft.User.IsDeleted);
    }
}