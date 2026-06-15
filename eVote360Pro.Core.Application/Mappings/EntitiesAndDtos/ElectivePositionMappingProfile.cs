using AutoMapper;
using eVote360Pro.Core.Application.Dtos.ElectivePosition;
using eVote360Pro.Core.Application.ViewModels.ElectivePosition;
using eVote360Pro.Core.Domain.Entities;

namespace eVote360Pro.Core.Application.Mappings.EntitiesAndDtos
{
    public class ElectivePositionMappingProfile : Profile
    {
        public ElectivePositionMappingProfile()
        {
            CreateMap<ElectivePosition, ElectivePositionDto>().ReverseMap();
            CreateMap<ElectivePosition, SaveElectivePositionViewModel>().ReverseMap();
        }
    }
}