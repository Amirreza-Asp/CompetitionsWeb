using Competitions.SharedKernel.ValueObjects;
using Competitions.SharedKernel.ValueObjects.Guards;

namespace Competitions.Domain.Entities.Places
{
    public class Supervisor : BaseEntity<long>
    {
        public Supervisor ( string name , PhoneNumber phoneNumber )
        {
            Name = Guard.Against.NullOrEmpty(name);
            PhoneNumber = Guard.Against.InvalidPhoneNumber(phoneNumber , nameof(PhoneNumber));
        }

        private Supervisor () { }

        public String Name { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }

        public Place Place { get; private set; }



        public Supervisor WithName ( String name )
        {
            Name = Guard.Against.NullOrEmpty(name);
            return this;
        }
        public Supervisor WithPhoneNumber ( PhoneNumber phoneNumber )
        {
            PhoneNumber = phoneNumber;
            return this;
        }
    }
}
