using AutoMapper;
using IMS.Application.Common.DTOs;
using IMS.Application.Common.Interfaces;
using IMS.Application.Modules.Auth.DTOs.Users;
using IMS.Application.Modules.Auth.Interfaces;
using IMS.Domain.Exceptions;

namespace IMS.Application.Modules.Auth.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UserService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PaginatedApiResponse<UserDto>> GetAllAsync(PaginationParamsDto paginationParams)
    {
        var query = _uow.Users.Query();

        var totalRecords = query.Count();

        var pagedUsers = query
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToList();

        return new PaginatedApiResponse<UserDto>(_mapper.Map<List<UserDto>>(pagedUsers), totalRecords,
            paginationParams.PageNumber,
            paginationParams.PageSize);
    }

    public async Task<UserDto> GetByIdAsync(Guid id)
    {
        var user = await _uow.Users.GetOneAsync(e => e.Id == id);
        if (user == null)
            throw new BusinessException("User not found.");

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> UpdateProfileAsync(Guid id, UpdateUserDto dto)
    {
        var user = await _uow.Users.GetOneAsync(e => e.Id == id);
        if (user == null)
            throw new BusinessException("User not found.");

        _mapper.Map(dto, user);

        _uow.Users.Update(user);
        await _uow.CommitAsync();

        return _mapper.Map<UserDto>(user);
    }

    public async Task<bool> ToggleStatusAsync(Guid id)
    {
        var user = await _uow.Users.GetOneAsync(e => e.Id == id);
        if (user == null)
            throw new BusinessException("User not found.");

        user.IsActive = !user.IsActive;

        _uow.Users.Update(user);
        await _uow.CommitAsync();

        return user.IsActive;
    }
}