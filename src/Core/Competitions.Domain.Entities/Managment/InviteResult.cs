namespace Competitions.Domain.Entities.Managment
{
    public class InviteResult : BaseEntity<long>
    {
        public InviteResult ( bool accepted , Guid inviteId )
        {
            Accepted = accepted;
            CreateDate = DateTime.Now;
            InviteId = Guard.Against.Default(inviteId);
        }

        public bool Accepted { get; private set; }
        public DateTime CreateDate { get; private set; }
        public Guid InviteId { get; private set; }

        public InviteToMatch Invite { get; private set; }
    }
}
