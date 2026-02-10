using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configrations;

public class SalesOrderItemConfig : IEntityTypeConfiguration<SalesOrderItem>
{
    public void Configure(EntityTypeBuilder<SalesOrderItem> b)
    {
        b.Property(i => i.Quantity)
            .HasPrecision(18, 2)
            .IsRequired();

        b.Property(i => i.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        b.HasOne(i => i.SalesOrder)
            .WithMany(o => o.Items)
            .HasForeignKey(i => i.SalesOrderId);

        b.HasOne(i => i.Product)
            .WithMany(p => p.SalesOrderItems)
            .HasForeignKey(i => i.ProductId);
    }
}
