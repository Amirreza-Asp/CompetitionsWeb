using Competitions.Domain.Entities.Managment.Events.Matches;
using Competitions.SharedKernel.ValueObjects;

namespace Competitions.Domain.Entities.Managment
{
    public class MatchConditions : BaseEntity<Guid>
    {
        public MatchConditions ( Document regulations , bool free , int payment , Guid matchId )
        {
            Id = Guid.NewGuid();
            Regulations = regulations;
            Free = free;
            Payment = Guard.Against.Negative(payment);
            MatchId = Guard.Against.Default(matchId);
        }

        private MatchConditions () { }

        public Document Regulations { get; private set; }
        public bool Free { get; private set; }
        public int Payment { get; private set; }
        public Guid MatchId { get; private set; }


        public Match Match { get; private set; }


        public MatchConditions WithFree ( bool free )
        {
            if ( free )
                Payment = 0;
            Free = free;
            return this;
        }
        public MatchConditions WithPayment ( int payment )
        {
            Payment = Free ? 0 : Guard.Against.Negative(payment);
            return this;
        }
        public MatchConditions WithRegulations ( Document regulations )
        {
            Regulations = regulations;
            return this;
        }


        public void SaveRegulations ()
        {
            Events.Add(new SaveMatchConditionsRegulationsEvent(Regulations , StaticEntitiesDetails.MatchConditionsRegulationsPath));
        }
        public void DeleteRegulations ()
        {
            Events.Add(new DeleteMatchConditionsRegulationsEvent(StaticEntitiesDetails.MatchConditionsRegulationsPath , Regulations.Name));
        }

    }
}
