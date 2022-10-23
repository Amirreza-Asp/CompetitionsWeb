namespace Competitions.Domain.Entities.Managment.Events.Matches
{
    public class DeleteMatchImageEvent : BaseDomainEvent
    {
        public DeleteMatchImageEvent ( string path , string name )
        {
            Path = Guard.Against.NullOrEmpty(path);
            Name = Guard.Against.NullOrEmpty(name);
        }

        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Path { get; private set; }
        public string Name { get; private set; }
    }
}
