using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configrations;

public class ReservationRequestsConfig : BaseEntityConfig<ReservationRequests>
{

    public override void Configure(EntityTypeBuilder<ReservationRequests> b)
    {
        base.Configure(b);
        
        
        b.Property(x => x.Quantity)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        b.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>();
        

        /* Relationships */
        
        b.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        /* Indexes */
        b.HasIndex(x => x.ProductId);
        b.HasIndex(x => x.Status);
        b.HasIndex(x => x.CreatedAt);

        /* Check Constraint */

        b.HasCheckConstraint("CK_ReservationRequest_Quantity_Positive", "[Quantity] > 0");
    }

}