using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configrations;
public class ProductConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> b)
    {
        b.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(150);

        b.Property(p => p.SKU)
            .IsRequired()
            .HasMaxLength(50);

        b.HasIndex(p => p.SKU)
            .IsUnique();

        b.Property(p => p.ReorderLevel)
            .HasDefaultValue(0);

        b.Property(p => p.IsActive)
            .HasDefaultValue(true);
        
        
        b.Property(p => p.UnitPrice)
            .HasPrecision(18, 2);
        
        b.Property(p => p.UnitOfMeasure)
            .IsRequired();
    }
}