using Competitions.Domain.Entities.Managment;

namespace Competitions.Domain.Dtos.Matches.Matches
{
    public class RegisterMatchListDto
    {
        public List<RegisterMatchDto> RegisterMatches { get; set; } = new List<RegisterMatchDto>();

        public int TeamCount { get; set; }

        public IEnumerable<MatchDocument> Documents { get; set; }


        public Guid MatchId { get; set; }
        public bool Gender { get; set; }
    }
}
