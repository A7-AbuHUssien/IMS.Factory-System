
using IMS.Application.Modules.Auth.DTOs.Roles;
using IMS.Application.Modules.Auth.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers.Auth;

[Route("api/roles/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IRoleService roleService;
    public RolesController(IRoleService roleService)
    {
        this.roleService = roleService;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await roleService.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id) => Ok(await roleService.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create(CreateRoleDto dto) 
    {
        var result = await roleService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateRoleDto dto)
    {
        await roleService.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await roleService.DeleteAsync(id);
        return NoContent();
    }
}