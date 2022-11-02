using Competitions.Domain.Entities.Static;

namespace Competitions.Domain.Entities.Managment
{
    public class MatchAudienceType
    {
        public MatchAudienceType ( Guid matchId , long audienceTypeId )
        {
            MatchId = Guard.Against.Default(matchId);
            AudienceTypeId = Guard.Against.NegativeOrZero(audienceTypeId);
        }

        private MatchAudienceType () { }

        public Guid MatchId { get; private set; }
        public long AudienceTypeId { get; private set; }


        public AudienceType AudienceType { get; private set; }
        public Match Match { get; private set; }
    }
}
