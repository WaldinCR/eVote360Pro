using AutoMapper;
using eVote360Pro.Core.Domain.Entities;
using eVote360Pro.Core.Application.Dtos.PoliticalParty;

namespace eVote360Pro.Core.Application.Mappings.EntitiesAndDtos
{
    public class PoliticalPartyMappingProfile : Profile
    {
        public PoliticalPartyMappingProfile()
        {
            CreateMap<PoliticalParty, PoliticalPartyDto>()
                .ReverseMap();

            CreateMap<PoliticalParty, SavePoliticalPartyDto>()
                .ReverseMap()
                .ForMember(dest => dest.Candidates, opt => opt.Ignore())
                .ForMember(dest => dest.PoliticalLeaderAssignment, opt => opt.Ignore())
                .ForMember(dest => dest.SentAllianceRequests, opt => opt.Ignore())
                .ForMember(dest => dest.ReceivedAllianceRequests, opt => opt.Ignore())
                .ForMember(dest => dest.AlliancesAsParty1, opt => opt.Ignore())
                .ForMember(dest => dest.AlliancesAsParty2, opt => opt.Ignore());
        }
    }
}