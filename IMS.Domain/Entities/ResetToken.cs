namespace IMS.Domain.Entities;

public class ResetToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    
    public string HashResetToken { get; set; } = null!;
    public bool IsUsed { get; set; }
    
    public DateTime ExpiresAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UsedAtUtc { get; set; }
    
    public User User { get; set; } = null!;
}