using Competitions.Domain.Entities.Sports;

namespace Competitions.Domain.Dtos.Sports.Coaches
{
    public class UpdateCoachDto : CreateCoachDto
    {
        public long Id { get; set; }

        public Coach UpdateEntity ( Coach entity )
        {
            entity.WithName(Name)
               .WithFamily(Family)
               .WithDescription(Description)
               .WithNationalCode(NationalCode)
               .WithPhoneNumber(PhoneNumber)
               .WithEducation(Education)
               .WithSportId(SportId)
               .WithCETId(CETId);

            return entity;
        }
    }
}
