using Competitions.Domain.Entities.Authentication;

namespace Competitions.Domain.Entities.Managment
{
    public class ExtracurricularUser : BaseEntity<long>
    {
        public ExtracurricularUser ( Guid userId , Guid extracurricularId )
        {
            IsPay = false;
            JoinTime = DateTime.Now;
            UserId = Guard.Against.Default(userId);
            ExtracurricularId = Guard.Against.Default(extracurricularId);
        }

        public bool IsPay { get; private set; }
        public DateTime JoinTime { get; private set; }
        public Guid UserId { get; private set; }
        public Guid ExtracurricularId { get; private set; }

        public Extracurricular Extracurricular { get; private set; }
        public User User { get; private set; }
    }
}
