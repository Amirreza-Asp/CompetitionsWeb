using Competitions.Domain.Entities.Managment;

namespace Competitions.Domain.Entities.Static
{
    public class Evidence : BaseEntity<long>
    {
        public Evidence ( string title , string? description )
        {
            Title = Guard.Against.NullOrEmpty(title);
            Description = description;
        }

        private Evidence () { }

        public string Title { get; private set; }
        public string? Description { get; private set; }

        public ICollection<MatchDocument> MatchDocuments { get; private set; }

        public Evidence WithTitle ( string title )
        {
            Title = Guard.Against.NullOrEmpty(title);
            return this;
        }
        public Evidence WithDescription ( string? description )
        {
            Description = description;
            return this;
        }
    }
}
