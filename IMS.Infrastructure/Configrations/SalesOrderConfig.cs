using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configrations;

public class SalesOrderConfig : IEntityTypeConfiguration<SalesOrder>
{
    public void Configure(EntityTypeBuilder<SalesOrder> b)
    {
        b.Property(o => o.Total)
            .HasPrecision(18, 2);
        b.Property(so => so.TaxAmount)
            .HasPrecision(18, 2);

        b.Property(o => o.Status)
            .IsRequired();

        b.Property(s=>s.Discount)
            .HasPrecision(18, 2);
        b.HasOne(o => o.Customer)
            .WithMany(c => c.SalesOrders)
            .HasForeignKey(o => o.CustomerId);
        
        b.HasIndex(so => so.OrderNumber).IsUnique();
        b.Property(so => so.OrderNumber).IsRequired().HasMaxLength(50);

        b.HasOne(so => so.Customer)
            .WithMany(c => c.SalesOrders)
            .HasForeignKey(so => so.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
        

