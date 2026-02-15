using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configrations;

public class SalesOrderConfig : BaseEntityConfig<SalesOrder>
{
    public override void Configure(EntityTypeBuilder<SalesOrder> b)
    {
        base.Configure(b); 
        b.HasIndex(o => o.OrderNumber).IsUnique();
        b.Property(o => o.OrderNumber).IsRequired().HasMaxLength(30)
            .HasDefaultValueSql("'SO-' + RIGHT('000000' + CAST(NEXT VALUE FOR SalesOrderSeq AS varchar(6)), 6)");

        b.Property(o => o.Status)
            .HasConversion<int>()
            .IsRequired();

        b.Property(o => o.TotalPrice).HasPrecision(18, 2);
        b.Property(o => o.TotalCost).HasPrecision(18, 2);

        b.Property(o => o.OrderDate)
            .HasDefaultValueSql("GETDATE()");

        b.HasOne(o => o.Customer)
            .WithMany(c => c.SalesOrders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        b.HasMany(o => o.Items)
            .WithOne(i => i.SalesOrder)
            .HasForeignKey(i => i.SalesOrderId)
            .OnDelete(DeleteBehavior.Cascade);
        
    }

}
        

