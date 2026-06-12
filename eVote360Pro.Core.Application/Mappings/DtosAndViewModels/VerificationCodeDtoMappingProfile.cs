using AutoMapper;
using eVote360Pro.Core.Application.Dtos.VerificationCode;
using eVote360Pro.Core.Application.ViewModels.Elector;

namespace eVote360Pro.Core.Application.Mappings.DtosAndViewModels
{
    public class VerificationCodeDtoMappingProfile : Profile
    {
        public VerificationCodeDtoMappingProfile()
        {
            // ValidarOtpViewModel // VerificationCodeDto
            CreateMap<ValidateOtpViewModel, VerificationCodeDto>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CitizenId, opt => opt.Ignore())
                .ForMember(dest => dest.ElectionId, opt => opt.Ignore())
                .ForMember(dest => dest.GeneratedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ExpiresAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsUsed, opt => opt.Ignore());
        }
    }
}