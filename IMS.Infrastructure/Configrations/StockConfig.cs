using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configrations;

public class StockConfig : IEntityTypeConfiguration<Stock>
{
    public void Configure(EntityTypeBuilder<Stock> b)
    {
        b.HasIndex(s => new { s.ProductId, s.WarehouseId }).IsUnique();

        b.HasIndex(s => new { s.ProductId, s.WarehouseId })
            .IsUnique();

        b.Property(s => s.Quantity)
            .HasPrecision(18, 2);

        b.Property(s => s.ReservedQuantity)
            .HasPrecision(18, 2);

        b.HasOne(s => s.Product)
            .WithMany(p => p.Stocks)
            .HasForeignKey(s => s.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(s => s.Warehouse)
            .WithMany(w => w.Stocks)
            .HasForeignKey(s => s.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);
        
        b.Property(s => s.MinQuantityLevel)
            .HasPrecision(18, 2);
    }
}
