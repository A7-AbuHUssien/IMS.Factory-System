using IMS.Application.Common.DTOs;
using IMS.Application.Modules.Auth.DTOs.Users;

namespace IMS.Application.Modules.Auth.Interfaces;

public interface IUserService
{
    Task<PaginatedApiResponse<UserDto>> GetAllAsync(PaginationParamsDto paginationParams);
    Task<UserDto> GetByIdAsync(Guid id);
    Task<UserDto> UpdateProfileAsync(Guid id, UpdateUserDto dto);
    Task<bool> ToggleStatusAsync(Guid id); 
}