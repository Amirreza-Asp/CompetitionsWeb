using Competitions.Domain.Entities.Managment.Events.Notifications;
using Competitions.SharedKernel.ValueObjects;

namespace Competitions.Domain.Entities.Managment
{
    public class Notification : BaseEntity<long>
    {
        public Notification ( string title , string description , Document image )
        {
            Title = Guard.Against.NullOrEmpty(title);
            Description = Guard.Against.NullOrEmpty(description);
            Image = image;
            CreateDate = DateTime.Now;
        }

        private Notification () { }

        public String Title { get; private set; }
        public String Description { get; private set; }
        public Document Image { get; private set; }
        public DateTime CreateDate { get; private set; }


        public Notification WithTitle ( String title )
        {
            Title = Guard.Against.NullOrEmpty(title);
            return this;
        }
        public Notification WithDescription ( String description )
        {
            Description = Guard.Against.NullOrEmpty(description);
            return this;
        }
        public Notification WithImage ( Document image )
        {
            Image = image;
            return this;
        }

        public void SaveImage ()
        {
            Events.Add(new SaveNotificationImageEvent(Image , StaticEntitiesDetails.NotificationPath));
        }

        public void DeleteImage ()
        {
            Events.Add(new DeleteNotificationImageEvent(StaticEntitiesDetails.NotificationPath , Image.Name));
        }

    }
}
