using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configrations;

public class CustomerConfig : BaseEntityConfig<Customer>
{
    public override void Configure(EntityTypeBuilder<Customer> b)
    {
        base.Configure(b);
        b.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        b.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(100);

        b.Property(c => c.Phone)
            .HasMaxLength(30);
        b.Property(c => c.Address)
            .HasMaxLength(100);
       
        b.HasIndex(p => p.Email)
            .IsUnique();
        b.HasIndex(p => p.Phone)
            .IsUnique();
        
        b.HasMany(c=>c.SalesOrders)
            .WithOne(c=>c.Customer)
            .HasForeignKey(c=>c.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
