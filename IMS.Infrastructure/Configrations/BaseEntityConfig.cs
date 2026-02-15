using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configrations;

public abstract class BaseEntityConfig<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> b)
    {
        b.Property(x => x.Id)
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        b.Property(x => x.CreatedAt)
            .HasColumnType("datetime2(0)")
            .HasDefaultValueSql("GETDATE()");

        b.Property(x => x.UpdatedAt)
            .HasColumnType("datetime2(0)")
            .HasDefaultValueSql("GETDATE()");

        b.Property(x => x.IsDeleted)
            .HasDefaultValue(false);
        
        b.HasQueryFilter(x => !x.IsDeleted);
        b.HasIndex(x => x.IsDeleted);
        b.HasIndex(x => new { x.IsDeleted, x.CreatedAt });
    }
}
