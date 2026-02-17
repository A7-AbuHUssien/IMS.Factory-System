using IMS.Application.Common.Interfaces;
using IMS.Application.Modules.Auth.DTOs;
using IMS.Domain.Constant;
using IMS.Domain.Entities;
using IMS.Domain.Exceptions;

namespace IMS.Application.Modules.Auth.UseCases;

public class RegisterUseCase
{
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtProvider _jwtProvider;

    public RegisterUseCase(IUnitOfWork unitOfWork, IPasswordHasher hasher, IJwtProvider jwtProvider)
    {
        _uow = unitOfWork;
        _hasher = hasher;
        _jwtProvider = jwtProvider;
    }

    public async Task<AuthResponseDto> Execute(RegisterRequestDto dto)
    {
        if (await _uow.Users.Any(e => e.NormalizedEmail == dto.Email.ToUpper()))
            throw new BusinessException("Email is Related to other Account.");
        if (await _uow.Users.Any(e => e.NormalizedUserName == dto.Username.ToUpper()))
            throw new BusinessException("username is already token, try other one.");
        await _uow.BeginTransactionAsync();
        try
        {
            var (passHash, salt) = _hasher.HashPassword(dto.Password);
            var user = new User()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                NormalizedEmail = dto.Email.ToUpper(),
                Username = dto.Username,
                NormalizedUserName = dto.Username.ToUpper(),
                PasswordHash = passHash,
                PasswordSalt = salt,
            };
            await _uow.Users.CreateAsync(user);
            var defaultRole = await _uow.Roles.GetOneAsync(e => e.Name == AppRoles.User);
            if (defaultRole is null) throw new BusinessException("No Role Found");
            var roles = new List<Role>() { defaultRole };

            await _uow.UserRoles.CreateAsync(new UserRole()
            {
                User = user,
                RoleId = defaultRole.Id
            });
            
            await _uow.CommitAsync();
            await _uow.CommitTransactionAsync();
            
            var token = _jwtProvider.Generate(user, roles);
            return new AuthResponseDto()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                Roles = roles.Select(r => r.Name)
            }; 
        }
        catch
        {
            await _uow.RollbackTransactionAsync();
            throw;
        }
    }
}