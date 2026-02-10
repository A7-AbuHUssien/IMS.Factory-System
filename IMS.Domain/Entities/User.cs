namespace IMS.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public bool IsActive { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
    public ICollection<StockTransaction> StockTransactions { get; set; }

}