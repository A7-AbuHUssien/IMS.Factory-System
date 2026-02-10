using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configrations;
public class StockTransactionConfig : IEntityTypeConfiguration<StockTransaction>
{
    public void Configure(EntityTypeBuilder<StockTransaction> b)
    {
        b.Property(t => t.Quantity)
            .HasPrecision(18, 2)
            .IsRequired();

        b.Property(t => t.Type)
            .IsRequired();

        b.HasOne(t => t.Product)
            .WithMany(p => p.StockTransactions)
            .HasForeignKey(t => t.ProductId);

        b.HasOne(t => t.Warehouse)
            .WithMany(w => w.StockTransactions)
            .HasForeignKey(t => t.WarehouseId);

        b.HasOne(t => t.User)
            .WithMany(u => u.StockTransactions)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
