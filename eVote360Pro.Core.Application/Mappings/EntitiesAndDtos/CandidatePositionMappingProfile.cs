using AutoMapper;
using eVote360Pro.Core.Application.ViewModels.CandidatePosition;
using eVote360Pro.Core.Domain.Entities;

namespace eVote360Pro.Core.Application.Mappings.EntitiesAndDtos
{
    public class CandidatePositionMappingProfile : Profile
    {
        public CandidatePositionMappingProfile()
        {
            CreateMap<CandidatePosition, SaveCandidatePositionViewModel>().ReverseMap();
        }
    }
}