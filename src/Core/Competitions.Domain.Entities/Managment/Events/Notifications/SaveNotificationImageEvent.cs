using Competitions.SharedKernel.ValueObjects;

namespace Competitions.Domain.Entities.Managment.Events.Notifications
{
    public class SaveNotificationImageEvent : BaseDomainEvent
    {
        public SaveNotificationImageEvent ( Document document , string path )
        {
            Document = document;
            Path = Guard.Against.NullOrEmpty(path);
        }

        public Guid Id { get; private set; } = Guid.NewGuid();
        public Document Document { get; private set; }
        public string Path { get; private set; }
    }
}
