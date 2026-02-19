using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configrations;

public class OTPConfig : IEntityTypeConfiguration<OTP>
{
    public void Configure(EntityTypeBuilder<OTP> builder)
    {
        // Table
        builder.ToTable("OTPs");

        // PK
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        // Properties
        builder.Property(x => x.Hash)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(x => x.ExpiresAtUtc)
            .IsRequired();

        builder.Property(x => x.IsUsed)
            .IsRequired();

        // Relationship
        builder.HasOne(x => x.User)
            .WithMany(u => u.OTPs)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes (performance + validation speed)
        builder.HasIndex(x => x.UserId);

        builder.HasIndex(x => x.ExpiresAtUtc);

        builder.HasIndex(x => new { x.UserId, x.IsUsed });
        
        builder.HasQueryFilter(o => !o.User.IsDeleted);
    }
}