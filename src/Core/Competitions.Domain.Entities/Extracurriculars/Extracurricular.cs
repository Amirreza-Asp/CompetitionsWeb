using Competitions.Domain.Entities.Managment.ValueObjects;
using Competitions.Domain.Entities.Places;
using Competitions.Domain.Entities.Sports;
using Competitions.Domain.Entities.Static;

namespace Competitions.Domain.Entities.Extracurriculars
{
    public class Extracurricular : BaseEntity<Guid>
    {
        public Extracurricular(string name, long sportId, Guid placeId, long audienceTypeId, int capacity, DateTimeRange putOn, bool gender, DateTimeRange register, string description, long coachId, int minimumPlacements, bool isFree)
        {
            Id = Guid.NewGuid();
            Name = Guard.Against.NullOrEmpty(name);
            SportId = Guard.Against.NegativeOrZero(sportId);
            PlaceId = Guard.Against.Default(placeId);
            AudienceTypeId = Guard.Against.NegativeOrZero(audienceTypeId);
            Capacity = Guard.Against.NegativeOrZero(capacity);
            PutOn = putOn;
            Gender = gender;
            Register = register;
            Description = Guard.Against.NullOrEmpty(description);
            CoachId = Guard.Against.NegativeOrZero(coachId);
            MinimumPlacements = minimumPlacements;
            IsFree = isFree;
        }

        private Extracurricular() { }

        public string Name { get; private set; }
        public long SportId { get; private set; }
        public Guid PlaceId { get; private set; }
        public long CoachId { get; private set; }
        public long AudienceTypeId { get; private set; }
        public int Capacity { get; private set; }
        public int MinimumPlacements { get; private set; }
        public DateTimeRange PutOn { get; private set; }
        public bool Gender { get; private set; }
        public DateTimeRange Register { get; private set; }
        public string Description { get; private set; }
        public bool IsFree { get; private set; }

        public Place Place { get; private set; }
        public Sport Sport { get; private set; }
        public AudienceType AudienceType { get; private set; }
        public Coach Coach { get; private set; }
        public ICollection<ExtracurricularTime> Times { get; private set; }
        public ICollection<ExtracurricularUser> Users { get; private set; }


        public Extracurricular WithName(string name)
        {
            Name = Guard.Against.NullOrEmpty(name);
            return this;
        }
        public Extracurricular WithSportId(long sportId)
        {
            SportId = Guard.Against.NegativeOrZero(sportId);
            return this;
        }
        public Extracurricular WithPlaceId(Guid placeId)
        {
            PlaceId = Guard.Against.Default(placeId);
            return this;
        }
        public Extracurricular WithCoachId(long coachId)
        {
            CoachId = Guard.Against.NegativeOrZero(coachId);
            return this;
        }
        public Extracurricular WithAudienceTypeId(long audienceTypeId)
        {
            AudienceTypeId = Guard.Against.NegativeOrZero(audienceTypeId);
            return this;
        }
        public Extracurricular WithCapacity(int capacity)
        {
            Capacity = Guard.Against.NegativeOrZero(capacity);
            return this;
        }
        public Extracurricular WithMinimumPlacements(int minimumPlacements)
        {
            MinimumPlacements = Guard.Against.NegativeOrZero(minimumPlacements);
            return this;
        }
        public Extracurricular WithPutOn(DateTimeRange putOn)
        {
            PutOn = putOn;
            return this;
        }
        public Extracurricular WithGender(bool gender)
        {
            Gender = gender;
            return this;
        }
        public Extracurricular WithRegister(DateTimeRange register)
        {
            Register = register;
            return this;
        }
        public Extracurricular WithDescripion(string description)
        {
            Description = Guard.Against.NullOrEmpty(description);
            return this;
        }
        public Extracurricular WithIsFree(bool isFree)
        {
            IsFree = isFree;
            return this;
        }


        public Extracurricular AddTime(ExtracurricularTime time)
        {
            if (Times == null)
                Times = new List<ExtracurricularTime>();

            Times.Add(time);
            return this;
        }

    }
}
