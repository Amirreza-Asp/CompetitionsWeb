using Competitions.Domain.Entities.Managment.Events.Matches;
using Competitions.SharedKernel.ValueObjects;

namespace Competitions.Domain.Entities.Managment
{
    public class UserTeamDocument : BaseEntity<long>
    {
        public UserTeamDocument ( string name , Document file , Guid userTeamId )
        {
            Name = Guard.Against.NullOrEmpty(name);
            UserTeamId = Guard.Against.Default(userTeamId);
            File = file;
            CreateDate = DateTime.Now;
        }

        private UserTeamDocument () { }

        public String Name { get; private set; }
        public Document File { get; private set; }
        public DateTime CreateDate { get; private set; }
        public Guid UserTeamId { get; private set; }

        public UserTeam UserTeam { get; private set; }


        public void SaveFile () => Events.Add(new SaveUserTeamDocEvent(File , StaticEntitiesDetails.UserTeamDocPath));
    }
}
