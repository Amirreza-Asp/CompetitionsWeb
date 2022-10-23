using Competitions.Domain.Entities.Managment;
using Competitions.Domain.Entities.Places;
using Competitions.Domain.Entities.Sports.Events;
using Competitions.SharedKernel.ValueObjects;

namespace Competitions.Domain.Entities.Sports
{
    public class Sport : BaseEntity<long>
    {

        public Sport ( string name , string? description , Document image , long sportTypeId )
        {
            Name = Guard.Against.NullOrEmpty(name);
            Description = description;
            Image = image;
            SportTypeId = Guard.Against.NegativeOrZero(sportTypeId);
            CreateDate = DateTime.Now;
        }

        private Sport () { }

        public String Name { get; private set; }
        public String? Description { get; private set; }
        public Document Image { get; private set; }
        public DateTime CreateDate { get; set; }
        public long SportTypeId { get; private set; }

        public SportType SportType { get; private set; }
        public ICollection<PlaceSports> Places { get; private set; }
        public ICollection<Coach>? Coaches { get; private set; }
        public ICollection<Extracurricular> Extracurriculars { get; private set; }
        public ICollection<Match> Matchs { get; private set; }

        public void SaveImage ()
        {
            Events.Add(new SaveSportImageEvent(Image , StaticEntitiesDetails.SportImagePath));
        }

        public void DeleteImage ()
        {
            Events.Add(new DeleteSportImageEvent(StaticEntitiesDetails.SportImagePath , Image.Name));
        }


        public Sport WithName ( String name )
        {
            Name = Guard.Against.NullOrEmpty(name);
            return this;
        }
        public Sport WithDescription ( String? description )
        {
            Description = description;
            return this;
        }
        public Sport WithImage ( Document image )
        {
            Image = image;
            return this;
        }
        public Sport WithSportTypeId ( long sportTypeId )
        {
            SportTypeId = Guard.Against.NegativeOrZero(sportTypeId);
            return this;
        }
    }
}
