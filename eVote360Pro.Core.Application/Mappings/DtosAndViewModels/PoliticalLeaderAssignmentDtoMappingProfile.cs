using AutoMapper;
using eVote360Pro.Core.Application.Dtos.PoliticalLeaderAssignment;
using eVote360Pro.Core.Application.ViewModels.PoliticalLeaderAssignment;

namespace eVote360Pro.Core.Application.Mappings.DtosAndViewModels
{
    public class PoliticalLeaderAssignmentDtoMappingProfile : Profile
    {
        public PoliticalLeaderAssignmentDtoMappingProfile()
        {
            CreateMap<PoliticalLeaderAssignmentDto, PoliticalLeaderAssignmentViewModel>()
                .ReverseMap();

            CreateMap<SavePoliticalLeaderAssignmentDto, SavePoliticalLeaderAssignmentViewModel>()
                .ReverseMap();
        }
    }
}
