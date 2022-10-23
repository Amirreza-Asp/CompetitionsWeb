namespace Competitions.Domain.Entities.Managment.Events.Festivals
{
    public class DeleteFestivalImageEvent : BaseDomainEvent
    {
        public DeleteFestivalImageEvent ( string path , string name )
        {
            Path = Guard.Against.NullOrEmpty(path);
            Name = Guard.Against.NullOrEmpty(name);
        }

        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Path { get; private set; }
        public string Name { get; private set; }
    }
}
