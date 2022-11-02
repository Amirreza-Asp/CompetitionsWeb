using Competitions.Domain.Entities.Notifications.Events;
using Competitions.SharedKernel.ValueObjects;

namespace Competitions.Domain.Entities.Notifications
{
    public class NotificationImage : BaseEntity<long>
    {
        public NotificationImage ( string name , Document image , long notificationId )
        {
            Name = Guard.Against.NullOrEmpty(name);
            Image = image;
            NotificationId = notificationId;
        }

        public NotificationImage ( string name , Document image )
        {
            Name = Guard.Against.NullOrEmpty(name);
            Image = image;
        }

        private NotificationImage () { }

        public String Name { get; private set; }
        public DateTime SendDate { get; private set; }
        public Document Image { get; private set; }
        public long NotificationId { get; private set; }

        public Notification Notification { get; private set; }


        public void SaveImage ()
        {
            Events.Add(new SaveNotificationImageEvent(Image , StaticEntitiesDetails.NotificationPath));
        }

        public void DeleteImage ()
        {
            Events.Add(new DeleteNotificationImageEvent(StaticEntitiesDetails.NotificationPath , Image.Name));
        }


        public NotificationImage WithNotificaiton ( Notification notification )
        {
            Notification = notification;
            return this;
        }
    }
}
