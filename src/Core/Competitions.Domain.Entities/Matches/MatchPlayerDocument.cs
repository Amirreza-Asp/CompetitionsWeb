using Competitions.SharedKernel.ValueObjects;

namespace Competitions.Domain.Entities.Managment
{
    public class MatchPlayerDocument : BaseEntity<long>
    {
        public Document File { get; private set; }
        public Guid MatchId { get; private set; }
        public Guid UserId { get; set; }


    }
}
