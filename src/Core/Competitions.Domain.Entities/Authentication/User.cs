using Competitions.Domain.Entities.Extracurriculars;
using Competitions.Domain.Entities.Managment;
using Competitions.SharedKernel.ValueObjects;

namespace Competitions.Domain.Entities.Authentication
{
    public class User : BaseEntity<Guid>
    {
        public User(string name, string family, PhoneNumber phoneNumber, NationalCode nationalCode, String userName, String password, Guid roleId, StudentNumber studentNumber, String college, bool gender)
        {
            Id = Guid.NewGuid();
            Name = Guard.Against.NullOrEmpty(name);
            Family = Guard.Against.NullOrEmpty(family);
            PhoneNumber = phoneNumber;
            NationalCode = nationalCode;
            UserName = Guard.Against.NullOrEmpty(userName);
            Password = Guard.Against.NullOrEmpty(password);
            CreateDate = DateTime.Now;
            RoleId = Guard.Against.Default(roleId);
            StudentNumber = studentNumber;
            Gender = gender;
            College = college;
        }

        private User()
        {
        }

        public String Name { get; private set; }
        public String Family { get; private set; }
        public DateTime CreateDate { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public NationalCode NationalCode { get; private set; }
        public String UserName { get; private set; }
        public String Password { get; private set; }
        public StudentNumber StudentNumber { get; private set; }
        public String College { get; private set; }
        public bool IsDeleted { get; private set; }
        public bool Gender { get; private set; }
        public Guid RoleId { get; private set; }

        public Role Role { get; private set; }
        public ICollection<UserTeam> Teams { get; private set; }
        public ICollection<ExtracurricularUser> Extracurriculars { get; private set; }

        public User WithPassword(String password)
        {
            Password = Guard.Against.NullOrEmpty(password);
            return this;
        }

        public User WithRole(Guid roleId)
        {
            RoleId = Guard.Against.Default(roleId);
            return this;
        }
        public User WithCollege(String college)
        {
            College = Guard.Against.NullOrEmpty(college);
            return this;
        }

        public User Delete()
        {
            IsDeleted = true;
            return this;
        }

    }
}
