using AutoMapper;
using Competitions.Domain.Dtos.Places.ActivityPlans;
using Competitions.Domain.Dtos.Places.Image;
using Competitions.Domain.Dtos.Places.Places;
using Competitions.Domain.Dtos.Places.Supervisor;
using Competitions.Domain.Entities.Places;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Competitions.Common.Places
{
    public class PlaceProfile : Profile
    {

        public PlaceProfile ()
        {
            CreateMap<Place , GetPlaceDto>()
                .ForMember(dto => dto.Image , entity => entity.MapFrom(u => u.Images.ElementAt(0).Image.Name))
                .ForMember(dto => dto.SupervisorName , entity => entity.MapFrom(u => u.Supervisor.Name));

            // Create Place
            CreateMap<Place , CreatePlaceDto>();
            CreateMap<Supervisor , CreateSupervisorDto>()
                .ForMember(dto => dto.PhoneNumber , entity => entity.MapFrom(u => u.PhoneNumber.Value));

            // Update Place
            CreateMap<Place , UpdatePlaceDto>()
                .ForMember(dto => dto.CurrentImages , entity => entity.MapFrom(u => u.Images))
                .ForMember(dto => dto.NewSports , entity => entity.MapFrom(u => String.Join(',' , u.Sports.Select(b => b.Sport.Id))));
            CreateMap<PlaceImages , PlaceImageDto>()
                .ForMember(dto => dto.Name , entity => entity.MapFrom(u => u.Image.Name));
            CreateMap<PlaceSports , SelectListItem>()
                .ForMember(dto => dto.Text , entity => entity.MapFrom(u => u.Sport.Name))
                .ForMember(dto => dto.Value , entity => entity.MapFrom(u => u.SportId));

            // Activity Plan
            CreateMap<ActivityPlan , ActivityPlanDto>()
                .ForMember(dto => dto.File , entity => entity.MapFrom(u => u.File.Name));
        }

    }
}
