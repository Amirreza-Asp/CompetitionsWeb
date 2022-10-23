namespace Competitions.Domain.Entities.Sports
{
    public class SportType : BaseEntity<long>
    {
        public SportType(string title, string? description)
        {
            Title = Guard.Against.NullOrEmpty(title);
            Description = description;
        }

        private SportType() { }

        public string Title { get; private set; }
        public string? Description { get; private set; }

        public ICollection<Sport>? Sports { get; set; }

        public SportType WithTitle(string title)
        {
            Title = Guard.Against.NullOrEmpty(title);
            return this;
        }
        public SportType WithDescription(string? description)
        {
            Description = description;
            return this;
        }
    }
}
