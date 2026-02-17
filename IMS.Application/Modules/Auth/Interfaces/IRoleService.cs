using IMS.Application.Modules.Auth.DTOs.Roles;

namespace IMS.Application.Modules.Auth.Interfaces;

public interface IRoleService
{
    Task<List<RoleDto>> GetAllAsync();
    Task<RoleDto> GetByIdAsync(Guid id);
    Task<RoleDto> CreateAsync(CreateRoleDto dto);
    Task UpdateAsync(Guid id, UpdateRoleDto dto);
    Task DeleteAsync(Guid id);
}