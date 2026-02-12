using AutoMapper;
using IMS.Application.Modules.Auth.DTOs.Role;
using IMS.Domain.Entities;

namespace IMS.Application.Modules.Auth.Profiles;

public class RoleProfile :  Profile
{
    public RoleProfile()
    {
        CreateMap<CreateRoleDto, Role>();

        CreateMap<UpdateRoleDto, Role>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Role, RoleDto>()
            .ForMember(dest => dest.UserRolesCount, 
                opt => opt.MapFrom(src => src.UserRoles != null ? src.UserRoles.Count : 0));
    }
}