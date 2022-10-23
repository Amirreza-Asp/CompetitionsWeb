using Competitions.Domain.Dtos.Sports.Coaches;

namespace Competitions.Web.Areas.Sports.Models.Coaches
{
    public class GetAllCoaches
    {
        public IEnumerable<GetCoachDto> Coaches { get; set; }
        public CoachFilter Filters { get; set; }
    }
}
