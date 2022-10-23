using Competitions.Domain.Entities.Managment;

namespace Competitions.Web.Models.Matches
{
    public class MatchListVM
    {
        public IEnumerable<Match> Matches { get; set; }
        public MatchFilterDto Filters { get; set; }
    }
}
