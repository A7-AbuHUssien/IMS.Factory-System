using IMS.Application.Modules.Auth.DTOs;
using IMS.Application.Modules.Auth.Interfaces;
using IMS.Application.Modules.Auth.UseCases;

namespace IMS.Application.Modules.Auth.Services;

public class AuthService : IAuthService
{
    private readonly RegisterUseCase _register;

    public AuthService(RegisterUseCase register)
    {
        _register = register;
    }

    public async Task<AuthResponseDto> Register(RegisterRequestDto dto) => await _register.Execute(dto);
    
}