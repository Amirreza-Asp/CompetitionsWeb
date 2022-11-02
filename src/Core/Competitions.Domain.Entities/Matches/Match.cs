using Competitions.Domain.Entities.Managment.Events.Matches;
using Competitions.Domain.Entities.Managment.ValueObjects;
using Competitions.Domain.Entities.Places;
using Competitions.Domain.Entities.Sports;
using Competitions.SharedKernel.ValueObjects;

namespace Competitions.Domain.Entities.Managment
{
    public class Match : BaseEntity<Guid>
    {
        public Match ( string name , Guid? festivalId , Guid placeId , long sportId , bool gender , string level , DateTimeRange register , int capacity , byte teamCount , DateTimeRange putOn , Document image , string? description )
        {
            Id = Guid.NewGuid();
            CreateDate = DateTime.Now;
            Name = Guard.Against.NullOrEmpty(name);
            FestivalId = festivalId;
            PlaceId = Guard.Against.Default(placeId);
            SportId = Guard.Against.NegativeOrZero(sportId);
            Gender = gender;
            Level = Guard.Against.NullOrEmpty(level);
            Register = register;
            Capacity = Guard.Against.NegativeOrZero(capacity);
            TeamCount = ( byte ) Guard.Against.NegativeOrZero(teamCount);
            PutOn = putOn;
            Image = image;
            Description = description;
        }

        private Match () { }

        public String Name { get; private set; }
        public bool Gender { get; private set; }
        public String Level { get; private set; }
        public int Capacity { get; private set; }
        public byte TeamCount { get; private set; }
        public int RegistrationsNumber { get; private set; }
        public DateTime CreateDate { get; private set; }
        public String? Description { get; private set; }
        public DateTimeRange Register { get; private set; }
        public DateTimeRange PutOn { get; private set; }
        public Document Image { get; private set; }

        public Guid? FestivalId { get; private set; }
        public Guid PlaceId { get; private set; }
        public long SportId { get; private set; }

        public Festival? Festival { get; private set; }
        public Place Place { get; private set; }
        public Sport Sport { get; private set; }
        public MatchConditions Conditions { get; private set; }
        public ICollection<MatchAward> Awards { get; private set; }
        public ICollection<MatchDocument> Documents { get; private set; }
        public ICollection<MatchAudienceType> AudienceTypes { get; private set; }
        public ICollection<Team> Teams { get; private set; }


        public Match WithName ( String name )
        {
            Name = Guard.Against.NullOrEmpty(name);
            return this;
        }
        public Match WithGender ( bool gender )
        {
            Gender = gender;
            return this;
        }
        public Match WithLevel ( String level )
        {
            Level = Guard.Against.NullOrEmpty(level);
            return this;
        }
        public Match WithCapacity ( int capacity )
        {
            Capacity = Guard.Against.NegativeOrZero(capacity);
            return this;
        }
        public Match WithTeamCount ( byte teamCount )
        {
            TeamCount = ( byte ) Guard.Against.NegativeOrZero(teamCount);
            return this;
        }
        public Match WithDescription ( String? description )
        {
            Description = description;
            return this;
        }
        public Match WithRegister ( DateTimeRange register )
        {
            Register = register;
            return this;
        }
        public Match WithPutOn ( DateTimeRange putOn )
        {
            PutOn = putOn;
            return this;
        }
        public Match WithImage ( Document image )
        {
            Image = image;
            return this;
        }
        public Match WithFestivalId ( Guid? festivalId )
        {
            FestivalId = festivalId;
            return this;
        }
        public Match WithPlaceId ( Guid placeId )
        {
            PlaceId = Guard.Against.Default(placeId);
            return this;
        }
        public Match WithSportId ( long sportId )
        {
            SportId = Guard.Against.NegativeOrZero(sportId);
            return this;
        }

        public void AddAward ( MatchAward award )
        {
            if ( Awards == null )
                Awards = new List<MatchAward>();

            Awards.Add(award);
        }
        public void AddDocument ( MatchDocument document )
        {
            if ( Documents == null )
                Documents = new List<MatchDocument>();

            Documents.Add(document);
        }
        public void AddAudienceType ( MatchAudienceType mat )
        {
            if ( AudienceTypes == null )
                AudienceTypes = new List<MatchAudienceType>();

            AudienceTypes.Add(mat);
        }

        public void SaveImage ()
        {
            Events.Add(new SaveMatchImageEvent(Image , StaticEntitiesDetails.MatchImagePath));
        }
        public void DeleteImage ()
        {
            Events.Add(new DeleteMatchImageEvent(StaticEntitiesDetails.MatchImagePath , Image.Name));
        }
    }
}
