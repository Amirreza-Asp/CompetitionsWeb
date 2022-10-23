using Competitions.Domain.Entities.Managment.Events.Festivals;
using Competitions.Domain.Entities.Managment.ValueObjects;
using Competitions.SharedKernel.ValueObjects;

namespace Competitions.Domain.Entities.Managment
{
    public class Festival : BaseEntity<Guid>
    {
        public Festival ( string title , DateTimeRange duration , string? description , Document image )
        {
            Id = Guid.NewGuid();
            Title = Guard.Against.NullOrEmpty(title);
            Duration = duration;
            Description = description;
            Image = image;
        }

        private Festival ()
        {
        }

        public String Title { get; private set; }
        public DateTimeRange Duration { get; private set; }
        public String? Description { get; private set; }
        public Document Image { get; set; }

        public ICollection<Match> Matchs { get; private set; }

        public Festival WithImage ( Document image )
        {
            Image = image;
            return this;
        }
        public Festival WithTitle ( String title )
        {
            Title = Guard.Against.NullOrEmpty(title);
            return this;
        }
        public Festival WithDuration ( DateTimeRange duration )
        {
            Duration = duration;
            return this;
        }
        public Festival WithDescription ( String? description )
        {
            Description = description;
            return this;
        }


        public void SaveImage ()
        {
            Events.Add(new SaveFestivalImageEvent(Image , StaticEntitiesDetails.FestivalImagePath));
        }

        public void DeleteImage ()
        {
            Events.Add(new DeleteFestivalImageEvent(StaticEntitiesDetails.FestivalImagePath , Image.Name));
        }

    }
}
