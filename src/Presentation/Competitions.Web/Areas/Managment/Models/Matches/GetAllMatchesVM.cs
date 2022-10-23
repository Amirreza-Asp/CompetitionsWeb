using Competitions.Domain.Dtos.Managment.Matches;

namespace Competitions.Web.Areas.Managment.Models.Matches
{
    public class GetAllMatchesVM
    {
        public IEnumerable<MatchDetailsDto> Matches { get; set; }
        public MatchFilter Filters { get; set; }
    }
}
