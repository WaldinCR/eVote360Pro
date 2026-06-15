using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Citizen;
using eVote360Pro.Core.Application.ViewModels.Citizen;

namespace eVote360Pro.Core.Application.Mappings.DtosAndViewModels
{
    public class CitizenDtoMappingProfile : Profile
    {
        public CitizenDtoMappingProfile()
        {
            CreateMap<CitizenDto, CitizenViewModel>().ReverseMap();
        }
    }
}