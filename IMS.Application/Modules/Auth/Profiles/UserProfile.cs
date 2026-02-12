using AutoMapper;
using IMS.Application.Modules.Auth.DTOs.User;
using IMS.Domain.Entities;

namespace IMS.Application.Modules.Auth.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.PasswordHash, otp => otp.Ignore());
        
        CreateMap<UpdateUserDto, User>()
            .ForMember(dest => dest.PasswordHash, otp => otp.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<User, UserDto>()
            .ForMember(dest => dest.UserRolesCount,
                opt => opt.MapFrom(src => src.UserRoles != null ? src.UserRoles.Count : 0))
            .ForMember(dest=>dest.StockTransactionsCount,
                otp => otp.MapFrom(src => src.StockTransactions != null ? src.StockTransactions.Count : 0));
        
    }
}