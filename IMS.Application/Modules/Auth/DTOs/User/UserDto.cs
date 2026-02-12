namespace IMS.Application.Modules.Auth.DTOs.User;

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public int UserRolesCount { get; set; }
    public int StockTransactionsCount { get; set; }
}