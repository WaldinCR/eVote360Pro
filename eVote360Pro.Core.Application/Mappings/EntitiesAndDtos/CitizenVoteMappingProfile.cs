using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Citizen;
using eVote360Pro.Core.Domain.Entities;

namespace eVote360Pro.Core.Application.Mappings.EntitiesAndDtos
{
    public class CitizenVoteMappingProfile : Profile
    {
        public CitizenVoteMappingProfile()
        {
            CreateMap<CitizenVote, CitizenVoteDto>().ReverseMap();
        }
    }
}
