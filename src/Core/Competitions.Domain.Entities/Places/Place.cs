using Competitions.Domain.Entities.Extracurriculars;
using Competitions.Domain.Entities.Managment;

namespace Competitions.Domain.Entities.Places
{
    public class Place : BaseEntity<Guid>
    {
        internal Place () { }

        public Place ( string title , long placeTypeId , string address , long meterage , Guid? parentPlaceId )
        {
            Id = Guid.NewGuid();
            Title = Guard.Against.NullOrEmpty(title);
            PlaceTypeId = Guard.Against.NegativeOrZero(placeTypeId);
            Address = Guard.Against.NullOrEmpty(address);
            Meterage = Guard.Against.NegativeOrZero(meterage);
            ParentPlaceId = parentPlaceId;

            Sports = new List<PlaceSports>();
            Images = new List<PlaceImages>();
            SubPlaces = new List<Place>();
        }

        public String Title { get; private set; }
        public long PlaceTypeId { get; private set; }
        public String Address { get; private set; }
        public long Meterage { get; private set; }
        public Guid? ParentPlaceId { get; private set; }
        public long SupervisorId { get; private set; }

        public Supervisor Supervisor { get; private set; }
        public Place ParentPlace { get; private set; }
        public PlaceType PlaceType { get; private set; }
        public ActivityPlan ActivityPlan { get; private set; }

        public ICollection<Place> SubPlaces { get; private set; }
        public ICollection<PlaceSports> Sports { get; private set; }
        public ICollection<PlaceImages> Images { get; private set; }
        public ICollection<Extracurricular> Extracurriculars { get; private set; }
        public ICollection<Match> Matchs { get; private set; }


        public Place WithSupervisor ( Supervisor supervisor )
        {
            Supervisor = supervisor;
            return this;
        }
        public Place WithTitle ( String title )
        {
            Title = Guard.Against.NullOrEmpty(title);
            return this;
        }
        public Place WithPlaceTypeId ( long placeTypeId )
        {
            PlaceTypeId = Guard.Against.NegativeOrZero(placeTypeId);
            return this;
        }
        public Place WithAddress ( String address )
        {
            Address = Guard.Against.NullOrEmpty(address);
            return this;
        }
        public Place WithMeterage ( long meterage )
        {
            Meterage = Guard.Against.NegativeOrZero(meterage);
            return this;
        }
        public Place WithParentId ( Guid? parentPlaceId )
        {
            ParentPlaceId = parentPlaceId;
            return this;
        }



        public void AddImage ( PlaceImages image )
        {
            if ( Images == null )
                Images = new List<PlaceImages>();
            Images.Add(image);
        }
        public void RemoveImage ( PlaceImages image ) => Images.Remove(image);

        public void AddSport ( PlaceSports sport ) => Sports.Add(sport);
        public void RemoveSport ( PlaceSports sport ) => Sports.Remove(sport);

        public void AddChild ( Place children ) => SubPlaces.Add(children);
        public void RemoveChild ( Place children ) => SubPlaces.Remove(children);

    }
}
