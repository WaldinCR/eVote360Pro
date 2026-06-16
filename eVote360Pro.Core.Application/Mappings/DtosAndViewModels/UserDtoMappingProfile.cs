using AutoMapper;
using eVote360Pro.Core.Application.Dtos.User;
using eVote360Pro.Core.Application.ViewModels.User; 

namespace eVote360Pro.Core.Application.Mappings.DtosAndViewModels
{
    public class UserDtoMappingProfile : Profile
    {
        public UserDtoMappingProfile()
        {
            // Mapeos de UserViewModel
            CreateMap<UserDto, UserViewModel>();
            CreateMap<UserViewModel, UserDto>();

            // Mapeos de SaveUserViewModel
            CreateMap<SaveUserDto, SaveUserViewModel>()
                .ForMember(dest => dest.ConfirmPassword, opt => opt.MapFrom(src => src.Password));

            CreateMap<SaveUserViewModel, SaveUserDto>();
        }
    }
}