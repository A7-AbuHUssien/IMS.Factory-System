using IMS.Application.Modules.Auth.DTOs;
using IMS.Application.Modules.Auth.Interfaces;
using IMS.Application.Modules.Auth.UseCases;

namespace IMS.Application.Modules.Auth.Services;

public class AuthService : IAuthService
{
    private readonly RegisterUseCase _register;
    private readonly LoginUseCase _login;
    private readonly RefreshTokenUseCase _refreshToken;
    public AuthService(RegisterUseCase register, LoginUseCase login, RefreshTokenUseCase refreshToken)
    {
        _register = register;
        _login = login;
        _refreshToken = refreshToken;
    }

    public async Task<bool> Register(RegisterRequestDto dto) => await _register.Execute(dto);
    public async Task<AuthResponseDto> Login(LoginRequestDto dto) => await  _login.Execute(dto);

    public async Task<RefreshTokenResponseDto> RefreshToken(RefreshTokenRequestDto dto)
        => await _refreshToken.Execute(dto);

}