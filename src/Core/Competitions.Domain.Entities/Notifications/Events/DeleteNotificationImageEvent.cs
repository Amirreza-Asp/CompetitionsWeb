namespace Competitions.Domain.Entities.Notifications.Events
{
    public class DeleteNotificationImageEvent : BaseDomainEvent
    {
        public DeleteNotificationImageEvent ( string path , string name )
        {
            Path = Guard.Against.NullOrEmpty(path);
            Name = Guard.Against.NullOrEmpty(name);
        }

        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Path { get; private set; }
        public string Name { get; private set; }
    }
}
