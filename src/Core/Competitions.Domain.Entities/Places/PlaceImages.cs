using Competitions.Domain.Entities.Places.Events;
using Competitions.SharedKernel.ValueObjects;

namespace Competitions.Domain.Entities.Places
{
    public class PlaceImages : BaseEntity<long>
    {
        public PlaceImages ( Guid placeId , Document image )
        {
            PlaceId = Guard.Against.Default(placeId);
            Image = image;
        }

        private PlaceImages () { }

        public Guid PlaceId { get; private set; }
        public Document Image { get; private set; }

        public Place Place { get; private set; }



        public void SaveImage ()
        {
            Events.Add(new SavePlaceImageEvent(Image , StaticEntitiesDetails.PlaceImagePath));
        }

        public void DeleteImage ()
        {
            Events.Add(new DeletePlaceImageEvent(StaticEntitiesDetails.PlaceImagePath , Image.Name));
        }
    }
}
