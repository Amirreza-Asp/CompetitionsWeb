using AutoMapper;
using Competitions.Domain.Dtos.Managment.Extracurriculars;
using Competitions.Domain.Dtos.Managment.Festivals;
using Competitions.Domain.Dtos.Managment.Matches;
using Competitions.Domain.Dtos.Managment.Notifications;
using Competitions.Domain.Entities.Managment;

namespace Competitions.Common.Managment
{
    public class ManagmentProfile : Profile
    {
        public ManagmentProfile ()
        {
            // Notification
            CreateMap<Notification , UpdateNotificationDto>()
                .ForMember(dto => dto.Image , entity => entity.MapFrom(b => b.Image.Name));


            // Extracurricular
            CreateMap<Extracurricular , GetExtracurricularDetailsDto>()
                .ForMember(dto => dto.Place , entity => entity.MapFrom(b => b.Place.Title))
                .ForMember(dto => dto.Sport , entity => entity.MapFrom(b => b.Sport.Name))
                .ForMember(dto => dto.AudienceType , entity => entity.MapFrom(b => b.AudienceType.Title));

            CreateMap<Extracurricular , UpdateExtracurricularDto>()
                .ForMember(dto => dto.StartRegister , entity => entity.MapFrom(b => b.Register.From))
                .ForMember(dto => dto.EndRegister , entity => entity.MapFrom(b => b.Register.To))
                .ForMember(dto => dto.StartPutOn , entity => entity.MapFrom(b => b.PutOn.From))
                .ForMember(dto => dto.EndPutOn , entity => entity.MapFrom(b => b.PutOn.To))
                .ForMember(dto => dto.Times , entity => entity.MapFrom(b => "[" + String.Join(',' , b.Times.Select(b => b.ToJson())) + "]"));

            // Festival
            CreateMap<Festival , UpdateFestivalDto>()
                .ForMember(dto => dto.Start , entity => entity.MapFrom(b => b.Duration.From))
                .ForMember(dto => dto.End , entity => entity.MapFrom(b => b.Duration.To))
                .ForMember(dto => dto.Image , entity => entity.MapFrom(b => b.Image.Name));


            // Match
            CreateMap<Match , MatchDetailsDto>()
                .ForMember(dto => dto.Gender , entity => entity.MapFrom(b => b.Gender ? "خانم" : "آقا"))
                .ForMember(dto => dto.Status , entity => entity.MapFrom(b => b.Register.From < DateTime.Now && b.Register.To > DateTime.Now ? "ثبت نام" :
                    b.PutOn.From > DateTime.Now ? "برگذار شده" : "انتظار"))
                .ForMember(dto => dto.Sport , entity => entity.MapFrom(b => b.Sport.Name));

            CreateMap<Match , MatchFirstInfoDto>()
                .ForMember(dto => dto.StartRegister , entity => entity.MapFrom(b => b.Register.From))
                .ForMember(dto => dto.EndRegister , entity => entity.MapFrom(b => b.Register.To))
                .ForMember(dto => dto.StartPutOn , entity => entity.MapFrom(b => GetDate(b.PutOn.From)))
                .ForMember(dto => dto.EndPutOn , entity => entity.MapFrom(b => GetDate(b.PutOn.To)));
        }

        private String GetDate ( DateTime date )
        {
            return date.Year + "/" + date.Month + "/" + date.Day + " " + date.TimeOfDay.ToString().Substring(0 , 5);
        }
    }
}
