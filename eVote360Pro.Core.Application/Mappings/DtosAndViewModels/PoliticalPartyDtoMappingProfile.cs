using AutoMapper;
using eVote360Pro.Core.Application.Dtos.PoliticalParty;
using eVote360Pro.Core.Application.ViewModels.PoliticalParty;

namespace eVote360Pro.Core.Application.Mappings.DtosAndViewModels
{
    public class PoliticalPartyDtoMappingProfile : Profile
    {
        public PoliticalPartyDtoMappingProfile()
        {
            CreateMap<PoliticalPartyDto, PoliticalPartyViewModel>().ReverseMap();
            CreateMap<SavePoliticalPartyDto, SavePoliticalPartyViewModel>().ReverseMap();
        }
    }
}