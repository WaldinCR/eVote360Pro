using AutoMapper;
using eVote360Pro.Core.Application.Dtos.User;
using eVote360Pro.Core.Domain.Entities;
using eVote360Pro.Core.Domain.Common.Enums;

namespace eVote360Pro.Core.Application.Mappings.EntitiesAndDtos
{
    public class UserMappingProfile: Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>()
                 .ForMember(dest => dest.Role, opt => opt.MapFrom(src => (int)src.Role))
                 .ReverseMap()
                 .ForMember(dest => dest.Password, opt => opt.Ignore())
                 .ForMember(dest => dest.Role, opt => opt.MapFrom(src => (UserRol)src.Role));

            CreateMap<User, SaveUserDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => (int)src.Role))
                .ReverseMap()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => (UserRol)src.Role));
        }
    }
}
