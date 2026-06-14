using AutoMapper;
using eVote360Pro.Core.Application.Dtos.PoliticalLeader;
using eVote360Pro.Core.Application.ViewModels.PoliticalLeader;

namespace eVote360Pro.Core.Application.Mappings.DtosAndViewModels
{
    public class PoliticalLeaderDtoMappingProfile : Profile
    {
        public PoliticalLeaderDtoMappingProfile()
        {
            // Mapeo entre DTO y ViewModel (para mostrar datos en la vista)
            CreateMap<PoliticalLeaderDto, PoliticalLeaderViewModel>().ReverseMap();
            CreateMap<SavePoliticalLeaderDto, SavePoliticalLeaderViewModel>().ReverseMap();
        }
    }
}