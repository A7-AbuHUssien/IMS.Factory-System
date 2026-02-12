using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configrations;
public class StockTransactionConfig : BaseEntityConfig<StockTransaction>
{
    public override void Configure(EntityTypeBuilder<StockTransaction> b)
    {
        base.Configure(b);

        b.Property(t => t.Quantity)
            .HasPrecision(18, 2)
            .IsRequired();

        b.Property(t => t.UnitCost)
            .HasPrecision(18, 4);

        b.Property(t => t.UnitPrice)
            .HasPrecision(18, 2);

        b.Property(t => t.BalanceAfter)
            .HasPrecision(18, 2);

        b.Property(t => t.Type)
            .HasConversion<int>()
            .IsRequired();

        b.Property(t => t.Source)
            .HasConversion<int>()
            .IsRequired();

        b.Property(t => t.ReferenceType)
            .HasMaxLength(50);

        b.Property(t => t.TransactionDate)
            .HasDefaultValueSql("GETDATE()");

        b.Ignore(t => t.TotalCost);

        // Indexes
        b.HasIndex(t => new { t.ProductId, t.WarehouseId, t.TransactionDate });
        b.HasIndex(t => new { t.ReferenceId, t.ReferenceType });

        // Relations
        b.HasOne(t => t.Product)
            .WithMany(p => p.StockTransactions)
            .HasForeignKey(t => t.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(t => t.Warehouse)
            .WithMany(w => w.StockTransactions)
            .HasForeignKey(t => t.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(t => t.User)
            .WithMany(u => u.StockTransactions)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }

}
