using IMS.Application.Modules.Auth.DTOs;

namespace IMS.Application.Modules.Auth.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> Register(RegisterRequestDto dto);
}