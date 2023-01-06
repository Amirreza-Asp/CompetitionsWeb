using Competitions.Domain.Entities.Authentication;

namespace Competitions.Domain.Entities.Managment
{
    public class UserTeam : BaseEntity<Guid>
    {
        public UserTeam(Guid teamId, Guid userId, bool leader)
        {
            Id = Guid.NewGuid();
            TeamId = Guard.Against.Default(teamId);
            UserId = Guard.Against.Default(userId);
            Leader = leader;
        }

        public Guid TeamId { get; private set; }
        public Guid UserId { get; private set; }
        public bool Leader { get; set; } = false;

        public Team Team { get; private set; }
        public User User { get; private set; }
        public ICollection<UserTeamDocument> Documents { get; set; }

        public void AddDoc(UserTeamDocument doc)
        {
            if (Documents == null)
                Documents = new List<UserTeamDocument>();
            Documents.Add(doc);
        }
    }
}
