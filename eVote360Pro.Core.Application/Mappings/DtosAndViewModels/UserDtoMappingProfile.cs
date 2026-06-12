using AutoMapper;
using eVote360Pro.Core.Application.Dtos.User;
using eVote360Pro.Core.Application.ViewModels.User;

namespace eVote360Pro.Core.Application.Mappings.DtosAndViewModels
{
    public class UserDtoMappingProfile: Profile
    {
        public UserDtoMappingProfile()
        {
            // 1. Mapear de UserDto hacia tu UserViewModel (para el listado de la tabla)
            CreateMap<UserDto, UserViewModel>()
                .ReverseMap();

            // 2. Mapear de SaveUserDto hacia tu SaveUserViewModel (para los formularios)
            CreateMap<SaveUserDto, SaveUserViewModel>()
                .ForMember(dest => dest.ConfirmPassword, opt => opt.MapFrom(src => src.Password))
                .ReverseMap();
            
        }
    }
}
