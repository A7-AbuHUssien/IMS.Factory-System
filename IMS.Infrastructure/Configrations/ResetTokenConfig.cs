using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configrations;

public class ResetTokenConfig : IEntityTypeConfiguration<ResetToken>
{
    public void Configure(EntityTypeBuilder<ResetToken> builder)
    {
        // Table
        builder.ToTable("ResetTokens");

        // PK
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.HashResetToken)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired();

        builder.Property(x => x.ExpiresAtUtc)
            .IsRequired();

        builder.Property(x => x.IsUsed)
            .IsRequired();

        // Indexes
        builder.HasIndex(x => x.HashResetToken)
            .IsUnique();

        builder.HasIndex(x => new { x.UserId, x.IsUsed });

        builder.HasIndex(x => x.ExpiresAtUtc);

        // Relationships
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(ur => !ur.User.IsDeleted);
    }
}