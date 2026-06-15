using AutoMapper;
using eVote360Pro.Core.Domain.Entities;
using eVote360Pro.Core.Application.Dtos.PoliticalLeaderAssignment;

namespace eVote360Pro.Core.Application.Mappings.EntitiesAndDtos
{
    public class PoliticalLeaderAssignmentMappingProfile : Profile
    {
        public PoliticalLeaderAssignmentMappingProfile()
        {
            CreateMap<PoliticalLeaderAssignment, PoliticalLeaderAssignmentDto>()
                .ForMember(dest => dest.PoliticalPartyName, opt => opt.MapFrom(src => src.PoliticalParty != null ? src.PoliticalParty.Name : "Desconocido"))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : string.Empty))
                .ReverseMap();

            CreateMap<PoliticalLeaderAssignment, SavePoliticalLeaderAssignmentDto>()
                .ReverseMap()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.PoliticalParty, opt => opt.Ignore());
        }
    }
}
