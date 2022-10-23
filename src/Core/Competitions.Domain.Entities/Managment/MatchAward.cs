namespace Competitions.Domain.Entities.Managment
{
    public class MatchAward : BaseEntity<long>
    {
        public MatchAward ( byte score , string prize , Guid matchId )
        {
            Score = ( byte ) Guard.Against.NegativeOrZero(score);
            Prize = Guard.Against.NullOrEmpty(prize);
            MatchId = Guard.Against.Default(matchId);
        }

        private MatchAward () { }

        public byte Score { get; private set; }
        public String Prize { get; private set; }
        public Guid MatchId { get; private set; }

        public Match Match { get; private set; }
    }
}
