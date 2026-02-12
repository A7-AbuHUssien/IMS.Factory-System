using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configrations;

public class InventoryAdjustmentConfig : BaseEntityConfig<InventoryAdjustment>
{
    public override void Configure(EntityTypeBuilder<InventoryAdjustment> b)
    {
        base.Configure(b);

        // ===== Basic Fields =====
        b.Property(x => x.AdjustmentDate)
            .HasDefaultValueSql("GETUTCDATE()");

        b.Property(x => x.Reason)
            .IsRequired()
            .HasMaxLength(500);

        // ===== Decimal Precision =====
        b.Property(x => x.QuantityBefore)
            .HasPrecision(18, 2);

        b.Property(x => x.QuantityAdjusted)
            .HasPrecision(18, 2);

        b.Property(x => x.QuantityAfter)
            .HasPrecision(18, 2);

        b.Property(x => x.CostImpact)
            .HasPrecision(18, 4);

        // ===== Indexes for Performance =====
        b.HasIndex(x => new { x.ProductId, x.WarehouseId, x.AdjustmentDate });

        b.HasIndex(x => x.AdjustmentDate);

        // ===== Relationships =====

        // مهم لحل تحذير الـ Global Filter مع Product
        b.HasOne(x => x.Product)
            .WithMany(p => p.InventoryAdjustments)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Warehouse)
            .WithMany()
            .HasForeignKey(x => x.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        // المستخدم اختياري
        b.HasOne(x => x.AdjustedByUser)
            .WithMany()
            .HasForeignKey(x => x.AdjustedByUserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
