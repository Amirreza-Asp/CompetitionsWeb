using Competitions.Domain.Entities.Static;

namespace Competitions.Domain.Entities.Managment
{
    public class MatchDocument : BaseEntity<long>
    {
        public MatchDocument ( string type , Guid matchId , long evidenceId )
        {
            Type = Guard.Against.NullOrEmpty(type);
            MatchId = Guard.Against.Default(matchId);
            EvidenceId = Guard.Against.NegativeOrZero(evidenceId);
        }

        private MatchDocument () { }

        public long EvidenceId { get; private set; }
        public String Type { get; private set; }
        public Guid MatchId { get; private set; }

        public Evidence Evidence { get; private set; }
        public Match Match { get; private set; }

        public String ToJson () => $"{{ evidenceId : {EvidenceId} , typeId : \"{Type}\"  }}";
    }
}
