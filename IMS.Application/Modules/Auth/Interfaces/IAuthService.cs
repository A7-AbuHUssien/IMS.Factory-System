using IMS.Application.Modules.Auth.DTOs;

namespace IMS.Application.Modules.Auth.Interfaces;

public interface IAuthService
{
    Task<RegisterResponseDto> Register(RegisterRequestDto dto);
    Task<LoginResponseDto> Login(LoginRequestDto dto);
    Task<RefreshTokenResponseDto> RefreshToken(RefreshTokenRequestDto dto);
    Task<MessageResponseDto> ForgotPassword(ForgotPasswordRequestDto dto);
    Task<VerifyOtpResponseDto> VerifyOtp(VerifyOtpRequestDto dto);
    Task<MessageResponseDto> ResetPassword(ResetPasswordRequestDto dto);
    Task<MessageResponseDto> ChangePassword(ChangePasswordRequestDto dto);
}