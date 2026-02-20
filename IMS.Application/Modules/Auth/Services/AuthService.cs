using IMS.Application.Modules.Auth.DTOs;
using IMS.Application.Modules.Auth.Interfaces;
using IMS.Application.Modules.Auth.UseCases;

namespace IMS.Application.Modules.Auth.Services;

public class AuthService : IAuthService
{
    private readonly RegisterUseCase _register;
    private readonly LoginUseCase _login;
    private readonly RefreshTokenUseCase _refreshToken;
    private readonly ForgotPasswordUseCase _forgotPassword;
    private readonly VerifyOtpUseCase _verifyOtp;
    private readonly ResetPasswordUseCase _resetPassword;
    private readonly ChangePasswordUseCase _changePassword;

    public AuthService(RegisterUseCase register, LoginUseCase login, RefreshTokenUseCase refreshToken,
        ForgotPasswordUseCase forgotPassword, VerifyOtpUseCase verifyOtp,
        ResetPasswordUseCase resetPassword, ChangePasswordUseCase changePassword)
    {
        _register = register;
        _login = login;
        _refreshToken = refreshToken;
        _forgotPassword = forgotPassword;
        _verifyOtp = verifyOtp;
        _resetPassword = resetPassword;
        _changePassword = changePassword;
    }

    public async Task<RegisterResponseDto> Register(RegisterRequestDto dto) => await _register.Execute(dto);
    public async Task<LoginResponseDto> Login(LoginRequestDto dto) => await _login.Execute(dto);

    public async Task<RefreshTokenResponseDto> RefreshToken(RefreshTokenRequestDto dto)
        => await _refreshToken.Execute(dto);

    public async Task<MessageResponseDto> ForgotPassword(ForgotPasswordRequestDto dto) =>
        await _forgotPassword.Execute(dto);

    public async Task<VerifyOtpResponseDto> VerifyOtp(VerifyOtpRequestDto dto) => await _verifyOtp.Execute(dto);

    public async Task<MessageResponseDto> ResetPassword(ResetPasswordRequestDto dto) =>
        await _resetPassword.Execute(dto);

    public async Task<MessageResponseDto> ChangePassword(ChangePasswordRequestDto dto) =>
        await _changePassword.Execute(dto);
}