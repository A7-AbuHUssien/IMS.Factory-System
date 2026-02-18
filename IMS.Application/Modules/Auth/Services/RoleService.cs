using AutoMapper;
using AutoMapper.QueryableExtensions;
using IMS.Application.Common.DTOs;
using IMS.Application.Common.Interfaces;
using IMS.Application.Modules.Auth.DTOs.Roles;
using IMS.Application.Modules.Auth.Interfaces;
using IMS.Domain.Entities;
using IMS.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Modules.Auth.Services;

public class RoleService(IUnitOfWork _uow,IMapper _mapper) : IRoleService
{
    public async Task<PaginatedApiResponse<RoleDto>> GetAllAsync(PaginationParamsDto pagination)
    {
        var query = _uow.Roles.Query();
        
        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ProjectTo<RoleDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        return new PaginatedApiResponse<RoleDto>(items, pagination.PageNumber, pagination.PageSize, totalCount, "Roles fetched");
    }

    public async Task<RoleDto> GetByIdAsync(Guid id)
    {
        var role = await _uow.Roles.GetOneAsync(e => e.Id == id);
        if (role == null) throw new BusinessException("RoleProfile not found.");
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
            throw new BusinessException("RoleProfile name already exists.");

        var role = new Role { Name = dto.Name, Description = dto.Description };

        await _uow.Roles.CreateAsync(role);
        await _uow.CommitAsync();

        return new RoleDto(){Id = role.Id, Name = role.Name, Description = role.Description};
    }

    public async Task<RoleDto> UpdateAsync(Guid id, UpdateRoleDto dto)
    {
        var role = await _uow.Roles.GetOneAsync(e=>e.Id == id);
        if (role == null) throw new BusinessException("RoleProfile not found.");

        if (await _uow.Roles.Any(r => r.Name.ToUpper() == dto.Name.ToUpper() && r.Id != id))
            throw new BusinessException("RoleProfile name already exists.");

        role.Name = dto.Name;
        role.Description = dto.Description;

        _uow.Roles.Update(role);
        await _uow.CommitAsync();
        return new RoleDto()
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description
        };
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var role = await _uow.Roles.GetOneAsync(e=>e.Id == id);
        if (role == null) throw new BusinessException("RoleProfile not found.");

        var isUsed = await _uow.UserRoles.Any(ur => ur.RoleId == id);
        if (isUsed) throw new BusinessException("Cannot delete role , it is assigned to users.");

        _uow.Roles.Delete(role);
        return await _uow.CommitAsync() > 0;
    }
}