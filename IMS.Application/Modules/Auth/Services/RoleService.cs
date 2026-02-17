using IMS.Application.Common.Interfaces;
using IMS.Application.Modules.Auth.DTOs.Roles;
using IMS.Application.Modules.Auth.Interfaces;
using IMS.Domain.Entities;
using IMS.Domain.Exceptions;

namespace IMS.Application.Modules.Auth.Services;

public class RoleService(IUnitOfWork _uow) : IRoleService
{
    public async Task<List<RoleDto>> GetAllAsync()
    {
        var roles = await _uow.Roles.GetAsync();
        var result = new List<RoleDto>();
        foreach (var role in roles)
            result.Add(new RoleDto() { Id = role.Id, Name = role.Name, Description = role.Description });
        return result;
    }

    public async Task<RoleDto> GetByIdAsync(Guid id)
    {
        var role = await _uow.Roles.GetOneAsync(e => e.Id == id);
        if (role == null) throw new BusinessException("Role not found.");
        return new RoleDto()
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description
        };
    }

    public async Task<RoleDto> CreateAsync(CreateRoleDto dto)
    {
        if (await _uow.Roles.Any(r => r.Name.ToUpper() == dto.Name.ToUpper()))
            throw new BusinessException("Role name already exists.");

        var role = new Role { Name = dto.Name, Description = dto.Description };

        await _uow.Roles.CreateAsync(role);
        await _uow.CommitAsync();

        return new RoleDto(){Id = role.Id, Name = role.Name, Description = role.Description};
    }

    public async Task UpdateAsync(Guid id, UpdateRoleDto dto)
    {
        var role = await _uow.Roles.GetOneAsync(e=>e.Id == id);
        if (role == null) throw new BusinessException("Role not found.");

        if (await _uow.Roles.Any(r => r.Name.ToUpper() == dto.Name.ToUpper() && r.Id != id))
            throw new BusinessException("Role name already exists.");

        role.Name = dto.Name;
        role.Description = dto.Description;

        _uow.Roles.Update(role);
        await _uow.CommitAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var role = await _uow.Roles.GetOneAsync(e=>e.Id == id);
        if (role == null) throw new BusinessException("Role not found.");

        var isUsed = await _uow.UserRoles.Any(ur => ur.RoleId == id);
        if (isUsed) throw new BusinessException("Cannot delete role because it is assigned to users.");

        _uow.Roles.Delete(role);
        await _uow.CommitAsync();
    }
}