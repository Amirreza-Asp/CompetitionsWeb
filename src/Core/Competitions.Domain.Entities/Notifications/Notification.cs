namespace Competitions.Domain.Entities.Notifications
{
    public class Notification : BaseEntity<long>
    {
        public Notification ( string title , string description )
        {
            Title = Guard.Against.NullOrEmpty(title);
            Description = Guard.Against.NullOrEmpty(description);
            CreateDate = DateTime.Now;
        }

        private Notification () { }

        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime CreateDate { get; private set; }


        public ICollection<NotificationImage> Images { get; private set; }

        public Notification WithTitle ( string title )
        {
            Title = Guard.Against.NullOrEmpty(title);
            return this;
        }
        public Notification WithDescription ( string description )
        {
            Description = Guard.Against.NullOrEmpty(description);
            return this;
        }


        public void AddImage ( NotificationImage image )
        {
            if ( Images == null )
                Images = new List<NotificationImage>();
            image.WithNotificaiton(this);
            Images.Add(image);
        }

        public void RemoveImage ( NotificationImage image )
        {
            if ( Images == null )
                return;

            Images.Remove(image);
        }


    }
}
