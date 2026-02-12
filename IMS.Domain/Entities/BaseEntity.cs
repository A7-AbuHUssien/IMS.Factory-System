using System.ComponentModel.DataAnnotations;

namespace IMS.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    
    public User? CreatedByUser { get; set; }
    public User? UpdatedByUser { get; set; }
}