using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.IdentityModel.Tokens;

namespace IMS.Infrastructure.Configrations;

public class ReturnenItemsConfig : IEntityTypeConfiguration<ReturnedItem>
{
    public void Configure(EntityTypeBuilder<ReturnedItem> b)
    {
        b.ToTable("ReturnedItems");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        b.Property(x => x.Quantity)
            .HasPrecision(18,2)
            .IsRequired();

        b.Property(x => x.Reason)
            .HasMaxLength(500);

        b.Property(x => x.Source)
            .HasMaxLength(50)
            .IsRequired();

        b.Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()")
            .IsRequired();

        b.HasIndex(x => x.ProductId);
        b.HasIndex(x => x.CreatedAt);

        b.HasCheckConstraint(
            "CK_ReturnedItems_Quantity_Positive",
            "[Quantity] > 0"
        );
    }
}