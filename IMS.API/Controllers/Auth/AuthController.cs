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
    public async Task<ApiResponse<RegisterResponseDto>> Register([FromBody] RegisterRequestDto dto)
    {
        return new ApiResponse<RegisterResponseDto>(await _auth.Register(dto));
    }

    [HttpPost("login")]
    public async Task<ApiResponse<LoginResponseDto>> Login([FromBody] LoginRequestDto dto)
    {
        return new ApiResponse<LoginResponseDto>(await _auth.Login(dto));
    }

    [HttpPost("refresh")]
    public async Task<ApiResponse<RefreshTokenResponseDto>> Refresh([FromBody] RefreshTokenRequestDto dto)
    {
        return new ApiResponse<RefreshTokenResponseDto>(await _auth.RefreshToken(dto));
    }

    [HttpPost("forgot-password")]
    public async Task<ApiResponse<MessageResponseDto>> ForgotPassword(ForgotPasswordRequestDto dto)
    {
        return new ApiResponse<MessageResponseDto>(await _auth.ForgotPassword(dto));
    }

    [HttpPost("verify-otp")]
    public async Task<ApiResponse<VerifyOtpResponseDto>> VerifyOtp(VerifyOtpRequestDto dto)
    {
        return new ApiResponse<VerifyOtpResponseDto>(await _auth.VerifyOtp(dto));
    }

    [HttpPost("reset-password")]
    public async Task<ApiResponse<MessageResponseDto>> ResetPassword(ResetPasswordRequestDto dto)
    {
        return new ApiResponse<MessageResponseDto>(await _auth.ResetPassword(dto));
    }
}