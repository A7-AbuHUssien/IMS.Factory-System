namespace IMS.Domain.Entities;

public class User : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public string Username { get; set; }
    public string NormalizedUserName { get; set; }
    public string Email { get; set; }
    public string NormalizedEmail { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public bool IsActive { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
}