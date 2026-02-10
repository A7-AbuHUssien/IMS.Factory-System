using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configrations;

public class CustomerConfig : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> b)
    {
        b.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(150);

        b.Property(c => c.Email)
            .HasMaxLength(100);

        b.Property(c => c.Phone)
            .HasMaxLength(30);
    }
}
