using Competitions.Domain.Entities.Places.Events;
using Competitions.SharedKernel.ValueObjects;

namespace Competitions.Domain.Entities.Places
{
    public class ActivityPlan : BaseEntity<long>
    {
        public ActivityPlan ( string name , Document file , Guid placeId )
        {
            Name = Guard.Against.NullOrEmpty(name);
            PlaceId = Guard.Against.Default(placeId);
            File = file;
        }

        private ActivityPlan () { }

        public String Name { get; private set; }
        public Document File { get; private set; }
        public Guid PlaceId { get; private set; }

        public Place Place { get; private set; }

        public void SaveFile ()
        {
            Events.Add(new AddActivityPlanFileEvent(File , StaticEntitiesDetails.ActivityPlanPath));
        }


        public void RemoveFile ()
        {
            Events.Add(new DeleteActivityPlanFileEvent(StaticEntitiesDetails.ActivityPlanPath , File.Name));
        }

    }
}
