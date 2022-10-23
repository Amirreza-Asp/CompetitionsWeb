using Competitions.Domain.Entities.Authentication;

namespace Competitions.Domain.Entities.Managment
{
    public class InviteToMatch : BaseEntity<Guid>
    {
        public InviteToMatch ( Guid inviterId , Guid invitedId , Guid matchId )
        {
            Id = Guid.NewGuid();
            IsRead = false;
            SendTime = DateTime.Now;
            InviterId = Guard.Against.Default(inviterId);
            InvitedId = Guard.Against.Default(invitedId);
            MatchId = Guard.Against.Default(matchId);
        }

        public DateTime SendTime { get; private set; }
        public bool IsRead { get; private set; }
        public Guid InviterId { get; private set; }
        public Guid InvitedId { get; private set; }
        public Guid MatchId { get; private set; }

        public User Inviter { get; private set; }
        public User Invited { get; private set; }
        public Match Match { get; private set; }
        public InviteResult InviteResult { get; private set; }
    }
}
