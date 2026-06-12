using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Vote;
using eVote360Pro.Core.Domain.Entities;

namespace eVote360Pro.Core.Application.Mappings.EntitiesAndDtos
{
    public class VoteMappingProfile : Profile
    {
        public VoteMappingProfile()
        {
            CreateMap<Vote, VoteDto>().ReverseMap();
            CreateMap<Vote, SaveVoteDto>().ReverseMap();
        }
    }
}
