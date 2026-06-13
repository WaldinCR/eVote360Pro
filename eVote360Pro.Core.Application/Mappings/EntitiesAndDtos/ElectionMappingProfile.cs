using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Election;
using eVote360Pro.Core.Application.ViewModels.Election;
using eVote360Pro.Core.Domain.Entities;

namespace eVote360Pro.Core.Application.Mappings.EntitiesAndDtos
{
    public class ElectionMappingProfile : Profile
    {
        public ElectionMappingProfile()
        {
            CreateMap<Election, ElectionDto>().ReverseMap();
            CreateMap<Election, SaveElectionViewModel>().ReverseMap();
        }
    }
}