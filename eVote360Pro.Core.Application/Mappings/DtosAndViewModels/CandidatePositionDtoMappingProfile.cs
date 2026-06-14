using AutoMapper;
using eVote360Pro.Core.Application.Dtos.CandidatePosition;
using eVote360Pro.Core.Application.ViewModels.CandidatePosition;

namespace eVote360Pro.Core.Application.Mappings.DtosAndViewModels
{
    public class CandidatePositionDtoMappingProfile : Profile
    {
        public CandidatePositionDtoMappingProfile()
        {
            CreateMap<CandidatePositionDto, CandidatePositionViewModel>().ReverseMap();
        }
    }
}