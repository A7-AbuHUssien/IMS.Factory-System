using IMS.Application.Common.Interfaces;
using IMS.Application.Modules.Auth.DTOs;
using IMS.Domain.Entities;

namespace IMS.Application.Modules.Auth.UseCases;

public class ForgotPasswordUseCase
{
    private readonly IUnitOfWork _uow;
    private readonly IEmailService _emailService;
    private readonly IOtpService _otpService;

    public ForgotPasswordUseCase(IUnitOfWork uow, IEmailService emailService, IOtpService otpService)
    {
        _uow = uow;
        _emailService = emailService;
        _otpService = otpService;
    }


    public async Task<MessageResponseDto> Execute(ForgotPasswordRequestDto dto)
    {
        var user = await _uow.Users.GetOneAsync(e => e.NormalizedEmail == dto.Email.ToUpper());
        if (user == null) return new MessageResponseDto() { Message = "Successfully Sent OTP" };
        await _uow.BeginTransactionAsync();
        try
        {
            var userOtPs =
                await _uow.OTPs.GetAsync(e => e.UserId == user.Id && !e.IsUsed && e.ExpiresAtUtc > DateTime.UtcNow);

            foreach (var userOtp in userOtPs)
                userOtp.IsUsed = true;


            var newOtp = _otpService.Generate();
            var otpHash = _otpService.Hash(newOtp);
            await _uow.OTPs.CreateAsync(new OTP()
            {
                IsUsed = false,
                ExpiresAtUtc = DateTime.UtcNow.AddMinutes(3),
                UserId = user.Id,
                Hash = otpHash
            });

            await _uow.CommitTransactionAsync();
            await _emailService.SendEmailAsync(user.Email, "Use This OTP To Reset Password. ", newOtp);
            return new MessageResponseDto() { Message = "Successfully Sent OTP" };
        }
        catch
        {
            await _uow.RollbackTransactionAsync();
            throw;
        }
    }
}