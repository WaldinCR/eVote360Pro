using AutoMapper;
using eVote360Pro.Core.Application.Dtos.ElectivePosition;
using eVote360Pro.Core.Application.ViewModels.ElectivePosition;

namespace eVote360Pro.Core.Application.Mappings.DtosAndViewModels
{
    public class ElectivePositionDtoMappingProfile : Profile
    {
        public ElectivePositionDtoMappingProfile()
        {
            CreateMap<ElectivePositionDto, ElectivePositionViewModel>().ReverseMap();
        }
    }
}