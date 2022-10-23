namespace Competitions.Domain.Entities.Sports
{
    public class CoachEvidenceType : BaseEntity<long>
    {
        public CoachEvidenceType ( string title , string? description )
        {
            Title = Guard.Against.NullOrEmpty(title);
            Description = description;
        }

        private CoachEvidenceType () { }

        public string Title { get; private set; }
        public string? Description { get; private set; }

        public ICollection<Coach>? Coaches { get; private set; }

        public CoachEvidenceType WithTitle ( string title )
        {
            Title = Guard.Against.NullOrEmpty(title);
            return this;
        }
        public CoachEvidenceType WithDescription ( string? description )
        {
            Description = description;
            return this;
        }
    }
}
