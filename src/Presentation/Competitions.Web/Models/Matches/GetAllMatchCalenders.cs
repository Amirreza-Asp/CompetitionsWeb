using Competitions.Domain.Entities.Managment;

namespace Competitions.Web.Models.Calenders
{
    public class GetAllMatchCalenders
    {
        public MatchCalenderFilter Filters { get; set; }
        public IEnumerable<Match> Matches { get; set; }
    }
}
