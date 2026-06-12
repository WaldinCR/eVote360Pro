using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Vote;
using eVote360Pro.Core.Application.ViewModels.Elector;

namespace eVote360Pro.Core.Application.Mappings.DtosAndViewModels
{
    public class ElectorDtoMappingProfile : Profile
    {
        public ElectorDtoMappingProfile()
        {
            // SaveVoteDto / ElectivePositionVoteViewModel
            // (para armar la lista de votos desde la pantalla de votación)
            CreateMap<ElectivePositionVoteViewModel, SaveVoteDto>()
                .ForMember(dest => dest.ElectivePositionId, opt => opt.MapFrom(src => src.ElectivePositionId))
                .ForMember(dest => dest.CandidateId, opt => opt.MapFrom(src => src.SelectedCandidateId))
                .ForMember(dest => dest.ElectionId, opt => opt.Ignore()); 
        }
    }
}
