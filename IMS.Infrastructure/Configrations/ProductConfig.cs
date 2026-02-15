using IMS.Domain.Entities;
using IMS.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configrations;
public class ProductConfig : BaseEntityConfig<Product>
{
    public override void Configure(EntityTypeBuilder<Product> b)
    {
        base.Configure(b);

        b.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(150);

        b.Property(p => p.SKU)
            .IsRequired()
            .HasMaxLength(20);

        b.Property(p => p.Description)
            .HasMaxLength(1000);

        b.HasIndex(p => p.SKU).IsUnique();

        b.Property(p => p.IsActive)
            .HasDefaultValue(true);

        b.Property(p => p.UnitOfMeasure)
            .HasConversion<int>()
            .IsRequired()
            .HasDefaultValue(UnitOfMeasure.Piece)
            .HasSentinel(UnitOfMeasure.Piece);

        b.Property(p => p.UnitPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        b.Property(p => p.AVGUnitCost)
            .HasPrecision(18, 2);
        // Relationships
        b.HasMany(p => p.Stocks)
            .WithOne(s => s.Product)
            .HasForeignKey(s => s.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasMany(p => p.StockTransactions)
            .WithOne(t => t.Product)
            .HasForeignKey(t => t.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasMany(p => p.SalesOrderItems)
            .WithOne(i => i.Product)
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasMany(p => p.InventoryAdjustments)
            .WithOne(a => a.Product)
            .HasForeignKey(a => a.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
