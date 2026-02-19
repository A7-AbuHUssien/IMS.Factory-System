namespace IMS.Application.Modules.Auth.DTOs;

public class LoginResponseDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime AccessTokenExpiresAtUtc { get; set; }

}