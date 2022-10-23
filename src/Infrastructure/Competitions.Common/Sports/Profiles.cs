using AutoMapper;
using Competitions.Domain.Dtos.Sports.Coaches;
using Competitions.Domain.Dtos.Sports.Sports;
using Competitions.Domain.Dtos.Sports.SportTypes;
using Competitions.Domain.Entities.Sports;

namespace Competitions.Common.Sports
{
    public class Profiles : Profile
    {
        public Profiles ()
        {
            // Sport Type
            CreateMap<SportType , CreateSportTypeDto>().ReverseMap();
            CreateMap<SportType , UpdateSportTypeDto>().ReverseMap();

            // Sport
            CreateMap<Sport , UpdateSportDto>()
                .ForMember(dto => dto.Image , entity => entity.MapFrom(u => u.Image.Name));

            CreateMap<Sport , GetSportDto>()
                .ForMember(dto => dto.Image , entity => entity.MapFrom(u => u.Image.Name));

            // Coach
            CreateMap<Coach , GetCoachDto>()
                .ForMember(dto => dto.FullName , entity => entity.MapFrom(u => String.Concat(u.Name , " " , u.Family)))
                .ForMember(dto => dto.PhoneNumber , entity => entity.MapFrom(u => u.PhoneNumber.Value))
                .ForMember(dto => dto.NationalCode , entity => entity.MapFrom(u => u.NationalCode.Value))
                .ForMember(dto => dto.Sport , entity => entity.MapFrom(u => u.Sport.Name));

            CreateMap<Coach , UpdateCoachDto>()
                .ForMember(dto => dto.PhoneNumber , entity => entity.MapFrom(u => u.PhoneNumber.Value))
                .ForMember(dto => dto.NationalCode , entity => entity.MapFrom(u => u.NationalCode.Value));
        }
    }
}
