namespace IMS.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string TokenHash { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresOn { get; set; }
    public bool IsUsed { get; set; }
    public bool IsRevoked { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
    public bool IsActive => !IsRevoked && !IsUsed && !IsExpired;
}