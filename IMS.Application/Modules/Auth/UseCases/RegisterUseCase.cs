using AutoMapper;
using IMS.Application.Common.Interfaces;
using IMS.Application.Modules.Auth.DTOs;
using IMS.Application.Modules.Auth.DTOs.Users;
using IMS.Domain.Constant;
using IMS.Domain.Entities;
using IMS.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Modules.Auth.UseCases;

public class RegisterUseCase
{
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtProvider _jwtProvider;
    private readonly IMapper _mapper;

    public RegisterUseCase(IUnitOfWork unitOfWork, IPasswordHasher hasher, IJwtProvider jwtProvider, IMapper mapper)
    {
        _uow = unitOfWork;
        _hasher = hasher;
        _jwtProvider = jwtProvider;
        _mapper = mapper;
    }

    public async Task<bool> Execute(RegisterRequestDto dto)
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
                IsActive = true
            };
            await _uow.Users.CreateAsync(user);
            var defaultRole = await _uow.Roles.GetOneAsync(e => e.Name == AppRoles.User);
            if (defaultRole is null) throw new BusinessException("No RoleProfile Found");
            var roles = new List<Role>() { defaultRole };

            await _uow.UserRoles.CreateAsync(new UserRole()
            {
                User = user,
                RoleId = defaultRole.Id
            });

            await _uow.CommitAsync();
            await _uow.CommitTransactionAsync();

            return true;
        }
        catch
        {
            await _uow.RollbackTransactionAsync();
            throw;
        }
    }
}