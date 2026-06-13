using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Citizen;
using eVote360Pro.Core.Application.ViewModels.Citizen;
using eVote360Pro.Core.Domain.Entities;

namespace eVote360Pro.Core.Application.Mappings.EntitiesAndDtos
{
    public class CitizenMappingProfile : Profile
    {
        public CitizenMappingProfile()
        {
            CreateMap<Citizen, CitizenDto>()
                .ForMember(dest => dest.Document, opt => opt.MapFrom(src => src.IdentificationNumber))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();

            CreateMap<Citizen, SaveCitizenViewModel>()
                .ForMember(dest => dest.Document, opt => opt.MapFrom(src => src.IdentificationNumber))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Name))
                .ReverseMap()
                .ForMember(dest => dest.IdentificationNumber, opt => opt.MapFrom(src => src.Document.Trim())); 
        }
    }
}