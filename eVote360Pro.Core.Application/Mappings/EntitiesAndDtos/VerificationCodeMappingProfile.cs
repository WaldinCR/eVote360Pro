using AutoMapper;
using eVote360Pro.Core.Application.Dtos.VerificationCode;
using eVote360Pro.Core.Domain.Entities;

namespace eVote360Pro.Core.Application.Mappings.EntitiesAndDtos
{
    public class VerificationCodeMappingProfile : Profile
    {
        public VerificationCodeMappingProfile()
        {
            CreateMap<VerificationCode, VerificationCodeDto>().ReverseMap();
        }
    }
}
