using AutoMapper;
using eVote360Pro.Core.Application.Dtos.PoliticalLeaderAssignment;
using eVote360Pro.Core.Domain.Entities;

namespace eVote360Pro.Core.Application.Mappings.EntitiesAndDtos
{
    public class PoliticalLeaderMappingProfile : Profile
    {
        public PoliticalLeaderMappingProfile()
        {
            CreateMap<PoliticalLeaderAssignment, PoliticalLeaderAssignmentDto>().ReverseMap();
            CreateMap<PoliticalLeaderAssignment, SavePoliticalLeaderAssignmentDto>().ReverseMap();
        }
    }
}