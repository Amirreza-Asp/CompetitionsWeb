namespace Competitions.Domain.Entities.Authentication
{
    public class Role : BaseEntity<Guid>
    {
        public Role ( String title , string display , string? description )
        {
            Id = Guid.NewGuid();
            Title = Guard.Against.NullOrEmpty(title);
            Description = description;
            Display = Guard.Against.NullOrEmpty(display);
        }

        public String Title { get; private set; }
        public String Display { get; private set; }
        public String? Description { get; private set; }


        public ICollection<User>? Users { get; private set; }
    }
}
