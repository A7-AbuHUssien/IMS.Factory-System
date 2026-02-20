using IMS.Application.Common.Interfaces;
using IMS.Application.Modules.Auth.DTOs;
using IMS.Application.Modules.Auth.Interfaces;
using IMS.Domain.Exceptions;

namespace IMS.Application.Modules.Auth.UseCases;

public class ChangePasswordUseCase
{
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher _hasher;
    private readonly ICurrentUser _currentUser;
    public ChangePasswordUseCase(IUnitOfWork uow, IPasswordHasher hasher, ICurrentUser currentUser)
    {
        _uow = uow;
        _hasher = hasher;
        _currentUser = currentUser;
    }
    public async Task<MessageResponseDto> Execute(ChangePasswordRequestDto dto)
    {
        var user = await _uow.Users.GetOneAsync(e => e.Id == _currentUser.Id);
        if (user == null) throw new BusinessException("Invalid Operation");
        if (!_hasher.VerifyPassword(dto.CurrentPassword, user.PasswordHash, user.PasswordSalt))
            throw new BusinessException("Invalid Credintials");
        var (newPassHash, salt) = _hasher.HashPassword(dto.NewPassword);
        user.PasswordHash = newPassHash;
        user.PasswordSalt = salt;
        
        _uow.Users.Update(user);
        var refreshToken = await _uow.RefreshTokens.GetOneAsync(x => x.UserId == user.Id && x.IsActive && x.IsRevoked == false);
        if (refreshToken == null) throw new BusinessException("Invalid Token");
        refreshToken.IsUsed = true;
        refreshToken.IsRevoked = true;
        _uow.RefreshTokens.Update(refreshToken);

        await _uow.CommitAsync();
        return new MessageResponseDto(){Message = "Successfully changed password"};
    }
}