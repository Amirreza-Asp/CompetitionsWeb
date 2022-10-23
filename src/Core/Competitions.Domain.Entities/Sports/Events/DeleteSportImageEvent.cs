namespace Competitions.Domain.Entities.Sports.Events
{
    public class DeleteSportImageEvent : BaseDomainEvent
    {
        public DeleteSportImageEvent ( string path , string name )
        {
            Path = Guard.Against.NullOrEmpty(path);
            Name = Guard.Against.NullOrEmpty(name);
        }

        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Path { get; private set; }
        public string Name { get; private set; }
    }
}
