using AutoMapper;
using IMS.Application.Common.Interfaces;
using IMS.Application.Modules.Auth.DTOs;
using IMS.Domain.Entities;
using IMS.Domain.Exceptions;

namespace IMS.Application.Modules.Auth.UseCases;

public class VerifyOtpUseCase
{
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtProvider _jwtProvider;
    private readonly IMapper _mapper;

    public VerifyOtpUseCase(IUnitOfWork unitOfWork, IPasswordHasher hasher, IJwtProvider jwtProvider, IMapper mapper)
    {
        _uow = unitOfWork;
        _hasher = hasher;
        _jwtProvider = jwtProvider;
        _mapper = mapper;
    }

    public async Task<VerifyOtpResponseDto> Execute(VerifyOtpRequestDto dto)
    {
        var user = await _uow.Users.GetOneAsync(e => e.NormalizedEmail == dto.Email.ToUpper());
        if (user == null) throw new BusinessException("Invalid OTP");
        var otp = await _uow.OTPs.GetOneAsync(e =>
            !e.IsUsed && e.UserId == user.Id && e.ExpiresAtUtc > DateTime.UtcNow);
        if (otp == null) throw new BusinessException("Invalid OTP");
        
        if (_hasher.Hash(dto.Otp) != otp.Hash) throw new BusinessException("Invalid OTP");
        
        await _uow.BeginTransactionAsync();
        try
        {
            otp.IsUsed = true;
            _uow.OTPs.Update(otp);

            var resetToken = _jwtProvider.GenerateRefreshToken();
            var resetTokenHash = _hasher.Hash(resetToken);

            await _uow.ResetTokens.CreateAsync(new ResetToken()
            {
                HashResetToken = resetTokenHash,
                ExpiresAtUtc = DateTime.UtcNow.AddMinutes(3),
                UserId = user.Id,
                IsUsed = false,
                CreatedAtUtc = DateTime.UtcNow
            });
            await _uow.CommitTransactionAsync();
            return new VerifyOtpResponseDto()
            {
                ResetToken = resetToken
            };
        }
        catch
        {
            await _uow.RollbackTransactionAsync();
            throw;
        }
    }
}