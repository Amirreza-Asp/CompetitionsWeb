using Competitions.Domain.Entities.Extracurriculars;
using Competitions.Domain.Entities.Managment;

namespace Competitions.Domain.Entities.Static
{
    public class AudienceType : BaseEntity<long>
    {
        public AudienceType(string title, string? description, bool isNeedInformation)
        {
            Title = Guard.Against.NullOrEmpty(title);
            IsNeedInformation = isNeedInformation;
            Description = description;
        }

        private AudienceType() { }

        public string Title { get; private set; }
        public bool IsNeedInformation { get; private set; }
        public string? Description { get; private set; }


        public ICollection<MatchAudienceType> Matchs { get; private set; }
        public ICollection<Extracurricular> Extracurriculars { get; private set; }

        public AudienceType WithTitle(string title)
        {
            Title = Guard.Against.NullOrEmpty(title);
            return this;
        }
        public AudienceType WithIsNeedInformation(bool isNeedInformation)
        {
            IsNeedInformation = isNeedInformation;
            return this;
        }
        public AudienceType WithDescription(string? description)
        {
            Description = description;
            return this;
        }

        public String ToJson() => $"{{ text : \"{Title}\" , value : \"{Id}\" }}";
    }
}
