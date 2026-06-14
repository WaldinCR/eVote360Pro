using AutoMapper;
using eVote360Pro.Core.Application.Dtos.PoliticalLeader;
using eVote360Pro.Core.Domain.Entities;

namespace eVote360Pro.Core.Application.Mappings.EntitiesAndDtos
{
    public class PoliticalLeaderMappingProfile : Profile
    {
        public PoliticalLeaderMappingProfile()
        {
            // Mapeo entre Entidad y DTO (para el mantenimiento del Dirigente)
            CreateMap<PoliticalLeader, PoliticalLeaderDto>().ReverseMap();
            CreateMap<PoliticalLeader, SavePoliticalLeaderDto>().ReverseMap();
        }
    }
}