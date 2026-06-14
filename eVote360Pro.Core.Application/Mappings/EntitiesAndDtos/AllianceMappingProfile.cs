using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Alliance;
using eVote360Pro.Core.Domain.Entities;

namespace eVote360Pro.Core.Application.Mappings.EntitiesAndDtos
{
    public class AllianceMappingProfile : Profile
    {
        public AllianceMappingProfile()
        {
            CreateMap<AllianceRequest, AllianceRequestDto>().ReverseMap();
            CreateMap<Alliance, AllianceDto>().ReverseMap();
        }
    }
}