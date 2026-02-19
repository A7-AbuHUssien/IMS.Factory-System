using IMS.Application.Common.Interfaces;
using IMS.Application.Modules.Auth.DTOs;

namespace IMS.Application.Modules.Auth.UseCases;

public class ResetPasswordUseCase
{
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher _passwordHasher;

    public ResetPasswordUseCase(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _uow = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<MessageResponseDto> Execute(ResetPasswordRequestDto dto)
    {
        var resetTokenHash = _passwordHasher.Hash(dto.ResetToken);
        var resetTokenInDb = await _uow.ResetTokens.GetOneAsync(e =>
            !e.IsUsed && e.ExpiresAtUtc > DateTime.UtcNow && e.HashResetToken == resetTokenHash);
        if (resetTokenInDb == null) return new MessageResponseDto() { Message = "Invalid Token" };
        var user = await _uow.Users.GetOneAsync(e => e.Id == resetTokenInDb.UserId);
        if (user == null) return new MessageResponseDto() { Message = "Invalid Token" };

        await _uow.BeginTransactionAsync();
        try
        {
            resetTokenInDb.IsUsed = true;
            resetTokenInDb.UsedAtUtc = DateTime.UtcNow;
            
            var (passHash, salt) = _passwordHasher.HashPassword(dto.NewPassword);
            user.PasswordHash = passHash;
            user.PasswordSalt = salt;
            
            _uow.ResetTokens.Update(resetTokenInDb);
            _uow.Users.Update(user);
            await _uow.CommitTransactionAsync();
            return new MessageResponseDto() { Message = "Password reset successful" };
        }
        catch
        {
            await _uow.RollbackTransactionAsync();
            throw;
        }
    }
}