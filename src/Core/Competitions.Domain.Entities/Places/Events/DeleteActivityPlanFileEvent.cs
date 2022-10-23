namespace Competitions.Domain.Entities.Places.Events
{
    public class DeleteActivityPlanFileEvent : BaseDomainEvent
    {
        public DeleteActivityPlanFileEvent ( string path , string name )
        {
            Path = Guard.Against.NullOrEmpty(path);
            Name = Guard.Against.NullOrEmpty(name);
        }

        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Path { get; private set; }
        public string Name { get; private set; }
    }
}
