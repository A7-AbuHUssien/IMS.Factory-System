using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configrations;

public class StockConfig : BaseEntityConfig<Stock>
{
    public override void Configure(EntityTypeBuilder<Stock> b)
    {
        base.Configure(b);

        b.HasIndex(s => new { s.ProductId, s.WarehouseId }).IsUnique();
        b.HasIndex(s => new { s.WarehouseId, s.ProductId });
        

        b.Property(s => s.Quantity)
            .HasPrecision(18, 2)
            .HasDefaultValue(0);

        b.Property(s => s.ReservedQuantity)
            .HasPrecision(18, 2)
            .HasDefaultValue(0);
        
        b.Property(s => s.AvgCost)
            .HasPrecision(18, 4)
            .HasDefaultValue(0);

        b.Ignore(s => s.AvailableQuantity);

        b.HasOne(s => s.Product)
            .WithMany(p => p.Stocks)
            .HasForeignKey(s => s.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(s => s.Warehouse)
            .WithMany(w => w.Stocks)
            .HasForeignKey(s => s.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);
    }

}
