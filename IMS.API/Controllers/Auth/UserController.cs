using IMS.Application.Common.DTOs;
using IMS.Application.Modules.Auth.DTOs.Users;
using IMS.Application.Modules.Auth.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers.Auth;

[Route("api/auth/[controller]")]
[ApiController]
public class UserController : ControllerBase
{

    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<PaginatedApiResponse<UserDto>> GetAll([FromQuery]PaginationParamsDto param)
    {
        return await _userService.GetAllAsync(param);
    }
    [HttpGet("{id}")]
    public async Task<ApiResponse<UserDto>> GetByIdAsync(Guid id)
    {
        return new ApiResponse<UserDto>(await _userService.GetByIdAsync(id));
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse<UserDto>> UpdateProfile(Guid id ,UpdateUserDto dto)
    {
        return new ApiResponse<UserDto>(await _userService.UpdateProfileAsync(id, dto));
    }
    [HttpPatch("{id}")]
    public async Task<ApiResponse<bool>> ToggleStatus(Guid id)
    {
        var newStatus = await _userService.ToggleStatusAsync(id);
        return new ApiResponse<bool>(newStatus);
    }
}