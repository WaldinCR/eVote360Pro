using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Alliance;
using eVote360Pro.Core.Application.ViewModels.Alliance;

namespace eVote360Pro.Core.Application.Mappings.DtosAndViewModels
{
    public class AllianceDtoMappingProfile : Profile
    {
        public AllianceDtoMappingProfile()
        {
            CreateMap<AllianceRequestDto, AllianceRequestViewModel>().ReverseMap();
            // Para la creación, el mapeo suele ser de un lado porque es un formulario de entrada
            CreateMap<CreateAllianceRequestViewModel, AllianceRequestDto>();
        }
    }
}