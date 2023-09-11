namespace Competitions.Domain.Entities.Managment
{
    public class Team : BaseEntity<Guid>
    {
        public Team(Guid matchId)
        {
            Id = Guid.NewGuid();
            MatchId = Guard.Against.Default(matchId);
            CreateDate = DateTime.Now;
        }

        private Team() { }

        public DateTime CreateDate { get; private set; }
        public Guid MatchId { get; private set; }

        public Match Match { get; private set; }
        public ICollection<UserTeam> Users { get; private set; }

        public void AddUser(UserTeam user)
        {
            if (Users == null)
                Users = new List<UserTeam>();

            Users.Add(user);
        }
    }
}
