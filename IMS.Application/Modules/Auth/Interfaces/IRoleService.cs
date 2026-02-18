using IMS.Application.Common.DTOs;
using IMS.Application.Modules.Auth.DTOs.Roles;

namespace IMS.Application.Modules.Auth.Interfaces;

public interface IRoleService
{
    Task<PaginatedApiResponse<RoleDto>> GetAllAsync(PaginationParamsDto pagination);
    Task<RoleDto> GetByIdAsync(Guid id);
    Task<RoleDto> CreateAsync(CreateRoleDto dto);
    Task<RoleDto> UpdateAsync(Guid id, UpdateRoleDto dto);
    Task<bool> DeleteAsync(Guid id);
}