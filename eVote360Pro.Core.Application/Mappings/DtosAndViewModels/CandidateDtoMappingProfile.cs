using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Candidate;
using eVote360Pro.Core.Application.ViewModels.Candidate;

namespace eVote360Pro.Core.Application.Mappings.DtosAndViewModels
{
    public class CandidateDtoMappingProfile : Profile
    {
        public CandidateDtoMappingProfile()
        {
            CreateMap<CandidateDto, CandidateViewModel>().ReverseMap();
            CreateMap<SaveCandidateDto, SaveCandidateViewModel>().ForMember(dest => dest.PhotoFile, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}