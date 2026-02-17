namespace IMS.Application.Modules.Auth.DTOs;

public class AuthResponseDto
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresAt  { get; set; }
    public IEnumerable<string> Roles { get; set; }
}