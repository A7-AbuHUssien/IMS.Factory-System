using AutoMapper;
using IMS.Application.Modules.Auth.DTOs.Roles;
using IMS.Application.Modules.Auth.DTOs.Users;
using IMS.Domain.Entities;

namespace IMS.Application.Modules.Auth.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
         CreateMap<User, UserDto>()
            .ForMember(dest => dest.Roles, 
                opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name)));

         CreateMap<UpdateUserDto, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());  


          CreateMap<Role, RoleDto>().ReverseMap();

         CreateMap<CreateRoleDto, Role>();

         CreateMap<UpdateRoleDto, Role>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}