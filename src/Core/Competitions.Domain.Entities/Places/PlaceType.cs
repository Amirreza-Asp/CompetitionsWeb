namespace Competitions.Domain.Entities.Places
{
    public class PlaceType : BaseEntity<long>
    {
        public PlaceType ( string title , string? description )
        {
            Title = Guard.Against.NullOrEmpty(title);
            Description = description;
        }

        private PlaceType () { }

        public string Title { get; private set; }
        public string? Description { get; private set; }


        public ICollection<Place> Places { get; set; }

        public PlaceType WithTitle ( string title )
        {
            Title = Guard.Against.NullOrEmpty(title);
            return this;
        }
        public PlaceType WithDescription ( string? description )
        {
            Description = description;
            return this;
        }
    }
}
