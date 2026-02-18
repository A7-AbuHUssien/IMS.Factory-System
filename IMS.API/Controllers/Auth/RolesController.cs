using IMS.Application.Common.DTOs;
using IMS.Application.Modules.Auth.DTOs.Roles;
using IMS.Application.Modules.Auth.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers.Auth;

[Route("api/roles/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<PaginatedApiResponse<RoleDto>> GetAll([FromQuery]PaginationParamsDto pagination)
    {
        return await _roleService.GetAllAsync(pagination);
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<RoleDto>> GetById(Guid id)
    {
        return new ApiResponse<RoleDto>(await _roleService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<ApiResponse<RoleDto>> Create(CreateRoleDto dto)
    {
        return new ApiResponse<RoleDto>(await _roleService.CreateAsync(dto));
    }


    [HttpPut("{id}")]
    public async Task<ApiResponse<RoleDto>> Update(Guid id, UpdateRoleDto dto)
    {
        return new ApiResponse<RoleDto>( await _roleService.UpdateAsync(id, dto));
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse<bool>> Delete(Guid id)
    {
        return new ApiResponse<bool>(await _roleService.DeleteAsync(id));
    }
}