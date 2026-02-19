using AutoMapper;
using IMS.Application.Common.Interfaces;
using IMS.Application.Modules.Auth.DTOs;
using IMS.Application.Modules.Auth.DTOs.Users;
using IMS.Domain.Entities;
using IMS.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Modules.Auth.UseCases;

public class LoginUseCase
{
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtProvider _jwtProvider;
    private readonly IMapper _mapper;

    public LoginUseCase(IUnitOfWork uow, IPasswordHasher hasher, IJwtProvider jwtProvider, IMapper mapper)
    {
        _uow = uow;
        _hasher = hasher;
        _jwtProvider = jwtProvider;
        _mapper = mapper;
    }

    public async Task<LoginResponseDto> Execute(LoginRequestDto dto)
    {
       var user = _uow.Users.Query().Include(e=>e.UserRoles).ThenInclude(e=>e.Role)
           .FirstOrDefault(e => e.NormalizedUserName == dto.EmailUsername || e.NormalizedEmail == dto.EmailUsername);
       
        if (user == null) throw new BusinessException("Invalid username or password");
        if (!user.IsActive) throw new BusinessException("User is blocked");
        if (!_hasher.VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt))
            throw new BusinessException("Invalid username or password");

        var accessToken = _jwtProvider.GenerateAccesToken(user, user.UserRoles.Select(ur => ur.Role));
        var refreshToken = _jwtProvider.GenerateRefreshToken();
        var refreshTokenHash = _hasher.Hash(refreshToken);
        
        
        await _uow.RefreshTokens.CreateAsync(new RefreshToken()
        {
            TokenHash = refreshTokenHash,
            UserId = user.Id,
            CreatedOn = DateTime.UtcNow,
            ExpiresOn = DateTime.UtcNow.AddDays(7),
            IsUsed = false,
            IsRevoked = false
        });
        await _uow.CommitAsync();
        return new LoginResponseDto()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiresAtUtc = DateTime.UtcNow.AddMinutes(15),
        };
    }
}