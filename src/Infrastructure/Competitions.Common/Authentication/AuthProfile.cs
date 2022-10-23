using AutoMapper;
using Competitions.Domain.Dtos.Authentication.User;
using Competitions.Domain.Entities.Authentication;

namespace Competitions.Common.Authentication
{
    public class AuthProfile : Profile
    {

        public AuthProfile ()
        {
            CreateMap<User , UserDetails>()
                .ForMember(dto => dto.FullName , entity => entity.MapFrom(u => u.Name + " " + u.Family))
                .ForMember(dto => dto.PhoneNumber , entity => entity.MapFrom(u => u.PhoneNumber.Value))
                .ForMember(dto => dto.NationalCode , entity => entity.MapFrom(u => u.NationalCode.Value))
                .ForMember(dto => dto.Role , entity => entity.MapFrom(u => u.Role.Display));


            CreateMap<User , UpdateUserDto>()
                .ForMember(dto => dto.FullName , entity => entity.MapFrom(u => u.Name + " " + u.Family))
                .ForMember(dto => dto.NationalCode , entity => entity.MapFrom(u => u.NationalCode.Value));
        }

    }
}
