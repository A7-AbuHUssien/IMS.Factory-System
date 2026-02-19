namespace IMS.Domain.Entities;

public class OTP
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Hash { get; set; } = null!;
    public DateTime ExpiresAtUtc { get; set; }
    public bool IsUsed { get; set; }
    
    public User User { get; set; }
}