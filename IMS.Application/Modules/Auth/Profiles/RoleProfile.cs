using AutoMapper;
using IMS.Application.Modules.Auth.DTOs.Roles;
using IMS.Domain.Entities;

namespace IMS.Application.Modules.Auth.Profiles;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<RoleDto, Role>();
        CreateMap<Role, RoleDto>();
    }
}