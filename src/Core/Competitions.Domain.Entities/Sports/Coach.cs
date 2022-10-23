using Competitions.Domain.Entities.Managment;
using Competitions.SharedKernel.ValueObjects;

namespace Competitions.Domain.Entities.Sports
{
    public class Coach : BaseEntity<long>
    {
        public Coach ( string name , string family , PhoneNumber phoneNumber , NationalCode nationalCode , long cETId , long sportId , string? description , string education )
        {
            Name = Guard.Against.NullOrEmpty(name);
            Family = Guard.Against.NullOrEmpty(family);
            PhoneNumber = phoneNumber;
            NationalCode = nationalCode;
            CETId = Guard.Against.NegativeOrZero(cETId);
            SportId = Guard.Against.NegativeOrZero(sportId);
            Description = description;
            Education = Guard.Against.NullOrEmpty(education);
        }

        private Coach () { }

        public String Name { get; private set; }
        public String Family { get; private set; }
        public String? Description { get; private set; }
        public String Education { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public NationalCode NationalCode { get; private set; }

        public long CETId { get; private set; }
        public long SportId { get; private set; }

        public CoachEvidenceType? CoachEvidenceType { get; private set; }
        public Sport? Sport { get; private set; }
        public ICollection<Extracurricular> Extracurriculars { get; private set; }



        public Coach WithName ( String name )
        {
            Name = Guard.Against.NullOrEmpty(name);
            return this;
        }
        public Coach WithFamily ( String family )
        {
            Family = Guard.Against.NullOrEmpty(family);
            return this;
        }
        public Coach WithEducation ( String education )
        {
            Education = Guard.Against.NullOrEmpty(education);
            return this;
        }
        public Coach WithDescription ( String? description )
        {
            Description = description;
            return this;
        }
        public Coach WithPhoneNumber ( PhoneNumber phoneNumber )
        {
            PhoneNumber = phoneNumber;
            return this;
        }
        public Coach WithNationalCode ( NationalCode nationalCode )
        {
            NationalCode = nationalCode;
            return this;
        }
        public Coach WithSportId ( long sportId )
        {
            SportId = Guard.Against.NegativeOrZero(sportId);
            return this;
        }
        public Coach WithCETId ( long cetId )
        {
            CETId = Guard.Against.NegativeOrZero(cetId);
            return this;
        }
    }
}
