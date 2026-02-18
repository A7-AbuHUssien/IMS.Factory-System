using IMS.Application.Modules.Auth.DTOs;

namespace IMS.Application.Modules.Auth.Interfaces;

public interface IAuthService
{
    Task<bool> Register(RegisterRequestDto dto);
    Task<AuthResponseDto> Login(LoginRequestDto dto);
    Task<RefreshTokenResponseDto> RefreshToken(RefreshTokenRequestDto dto);
}