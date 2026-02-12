using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configrations;

public class WarehouseConfig : BaseEntityConfig<Warehouse>
{
    public override void Configure(EntityTypeBuilder<Warehouse> b)
    {
        base.Configure(b);

        b.Property(w => w.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        b.HasIndex(w=>w.Name)
            .IsUnique();
        
        b.Property(w => w.Location)
            .IsRequired()
            .HasMaxLength(200);
        
        b.Property(w => w.IsActive)
            .HasDefaultValue(true);

        b.HasIndex(w => w.Name);
        b.HasIndex(w => w.IsActive);

        b.HasMany(w => w.Stocks)
            .WithOne(s => s.Warehouse)
            .HasForeignKey(s => s.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);
        
        b.HasMany(w => w.StockTransactions)
            .WithOne(t => t.Warehouse)
            .HasForeignKey(t => t.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}