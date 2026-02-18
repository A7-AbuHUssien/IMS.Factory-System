using IMS.Application.Common.Interfaces;
using IMS.Application.Modules.Auth.DTOs;
using IMS.Domain.Entities;
using IMS.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Modules.Auth.UseCases;

public class RefreshTokenUseCase
{
    private readonly IUnitOfWork _uow;
    private readonly IJwtProvider _jwtProvider;
    private readonly IPasswordHasher _hasher;

    public RefreshTokenUseCase(IUnitOfWork uow, IJwtProvider jwtProvider, IPasswordHasher hasher)
    {
        _uow = uow;
        _jwtProvider = jwtProvider;
        _hasher = hasher;
    }

    public async Task<RefreshTokenResponseDto> Execute(RefreshTokenRequestDto requestDto)
    {
        var token = await _uow.RefreshTokens.GetOneAsync(e => e.TokenHash == _hasher.Hash(requestDto.RefreshToken));

        if (token == null)
            throw new BusinessException("Invalid refresh token");

        if (token.IsUsed || token.IsRevoked || token.IsExpired)
            throw new BusinessException("Refresh token expired or invalid");

        var user = _uow.Users.Query().Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .FirstOrDefault(u => u.Id == token.UserId);
        
        if (user == null) throw new Exception("User not found");
        
        await _uow.BeginTransactionAsync();
        try
        {
            token.IsRevoked = true;
            token.IsUsed = true;

            var newAccess = _jwtProvider.GenerateAccesToken(user, user.UserRoles.Select(ur => ur.Role));
            var newRefresh = _jwtProvider.GenerateRefreshToken();
            var newHashedRefreshToken = _hasher.Hash(newRefresh);
            await _uow.RefreshTokens.CreateAsync(new RefreshToken()
            {
                TokenHash = newHashedRefreshToken,
                UserId = user.Id,
                CreatedOn = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddDays(7),
                IsUsed = false,
                IsRevoked = false
            });
            _uow.RefreshTokens.Update(token);
            await _uow.CommitAsync();
            await _uow.CommitTransactionAsync();
            return new RefreshTokenResponseDto()
            {
                AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(15),
                RefreshToken = newRefresh,
                AccessToken = newAccess,
            };
        }
        catch
        {
            await _uow.RollbackTransactionAsync();
            throw;
        }
        
       
    }
}