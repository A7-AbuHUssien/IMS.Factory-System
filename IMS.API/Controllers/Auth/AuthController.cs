using IMS.Application.Common.DTOs;
using IMS.Application.Modules.Auth.DTOs;
using IMS.Application.Modules.Auth.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers.Auth;

[Route("api/auth/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService authService)
    {
        _auth = authService;
    }

    [HttpPost("register")]
    public async Task<ApiResponse<bool>> Register([FromBody] RegisterRequestDto dto)
    {
        return new ApiResponse<bool>(await _auth.Register(dto));
    }

    [HttpPost("login")]
    public async Task<ApiResponse<AuthResponseDto>> Login([FromBody] LoginRequestDto dto)
    {
        return new ApiResponse<AuthResponseDto>(await _auth.Login(dto));
    }

    [HttpPost("refresh")]
    public async Task<ApiResponse<RefreshTokenResponseDto>> Refresh([FromBody] RefreshTokenRequestDto dto)
    {
        return new ApiResponse<RefreshTokenResponseDto>(await _auth.RefreshToken(dto));
    }
}