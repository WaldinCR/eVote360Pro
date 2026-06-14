using AutoMapper;
using eVote360Pro.Core.Domain.Entities;
using eVote360Pro.Core.Application.Dtos.Candidate;

namespace eVote360Pro.Core.Application.Mappings.EntitiesAndDtos
{
    public class CandidateMappingProfile : Profile
    {
        public CandidateMappingProfile()
        {
            CreateMap<Candidate, CandidateDto>()
                .ForMember(dest => dest.PoliticalPartyName, opt => opt.MapFrom(src => src.PoliticalParty!.Name))
                .ReverseMap();

            CreateMap<Candidate, SaveCandidateDto>()
                .ReverseMap()
                .ForMember(dest => dest.PoliticalParty, opt => opt.Ignore());
        }
    }
}