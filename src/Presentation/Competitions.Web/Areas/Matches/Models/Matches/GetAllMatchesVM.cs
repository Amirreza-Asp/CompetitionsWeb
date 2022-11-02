using Competitions.Domain.Dtos.Matches.Matches;

namespace Competitions.Web.Areas.Managment.Models.Matches
{
    public class GetAllMatchesVM
    {
        public IEnumerable<MatchDetailsDto> Matches { get; set; }
        public MatchFilter Filters { get; set; }
    }
}
