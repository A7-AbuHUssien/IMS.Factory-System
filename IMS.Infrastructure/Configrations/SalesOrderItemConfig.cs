using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configrations;

public class SalesOrderItemConfig : BaseEntityConfig<SalesOrderItem>
{
    public override void Configure(EntityTypeBuilder<SalesOrderItem> b)
    {
        base.Configure(b);

        b.Property(i => i.Quantity)
            .HasPrecision(18, 2)
            .IsRequired();

        b.Property(i => i.UnitPriceAtSale)
            .HasPrecision(18, 2)
            .IsRequired();

        b.Property(i => i.UnitCostAtSale)
            .HasPrecision(18, 2)
            .IsRequired();

        b.Ignore(i => i.LineProfit);

        b.HasIndex(i => new { i.Id, i.ProductId })
            .IsUnique();

        b.HasOne(i => i.SalesOrder)
            .WithMany(o => o.Items)
            .HasForeignKey(i => i.SalesOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(i => i.Product)
            .WithMany(p => p.SalesOrderItems)
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

      
    }
}
