using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Election;
using eVote360Pro.Core.Application.ViewModels.Election;

namespace eVote360Pro.Core.Application.Mappings.DtosAndViewModels
{
    public class ElectionDtoMappingProfile : Profile
    {
        public ElectionDtoMappingProfile()
        {
            CreateMap<ElectionDto, ElectionViewModel>().ReverseMap();
        }
    }
}