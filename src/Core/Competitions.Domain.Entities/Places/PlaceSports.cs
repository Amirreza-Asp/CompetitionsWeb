using Competitions.Domain.Entities.Sports;

namespace Competitions.Domain.Entities.Places
{
    public class PlaceSports
    {
        public PlaceSports ( long sportId , Guid placeId )
        {
            SportId = Guard.Against.NegativeOrZero(sportId);
            PlaceId = Guard.Against.Default(placeId);
        }

        private PlaceSports () { }

        public long SportId { get; private set; }
        public Guid PlaceId { get; private set; }

        public Place Place { get; private set; }
        public Sport Sport { get; private set; }
    }
}
