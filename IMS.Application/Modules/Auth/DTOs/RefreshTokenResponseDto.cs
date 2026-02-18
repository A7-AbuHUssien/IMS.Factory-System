namespace IMS.Application.Modules.Auth.DTOs;

public class RefreshTokenResponseDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime AccessTokenExpiresAt { get; set; }
}