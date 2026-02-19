using IMS.Application.Common.Interfaces;
using IMS.Application.Modules.Auth.DTOs;
using IMS.Domain.Constant;
using IMS.Domain.Entities;
using IMS.Domain.Exceptions;

namespace IMS.Application.Modules.Auth.UseCases;

public class RegisterUseCase(IUnitOfWork unitOfWork, IPasswordHasher hasher)
{
    public async Task<RegisterResponseDto> Execute(RegisterRequestDto dto)
    {
        if(dto.ConfirmPassword != dto.Password)  throw new BusinessException("Passwords do not match");
        if (await unitOfWork.Users.Any(e => e.NormalizedEmail == dto.Email.ToUpper()))
            throw new BusinessException("Email is Related to other Account.");
        if (await unitOfWork.Users.Any(e => e.NormalizedUserName == dto.Username.ToUpper()))
            throw new BusinessException("username is already token, try other one.");
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var (passHash, salt) = hasher.HashPassword(dto.Password);
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
                IsActive = true
            };
            await unitOfWork.Users.CreateAsync(user);
            var defaultRole = await unitOfWork.Roles.GetOneAsync(e => e.Name == AppRoles.User);
            if (defaultRole is null) throw new BusinessException("No RoleProfile Found");

            await unitOfWork.UserRoles.CreateAsync(new UserRole()
            {
                User = user,
                RoleId = defaultRole.Id
            });

            await unitOfWork.CommitAsync();
            await unitOfWork.CommitTransactionAsync();

            return new RegisterResponseDto()
            {
                Email = user.Email,
            };
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}