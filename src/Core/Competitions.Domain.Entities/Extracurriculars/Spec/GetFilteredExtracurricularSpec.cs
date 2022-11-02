using Competitions.Domain.Entities.Extracurriculars;

namespace Competitions.Domain.Entities.Extracurriculars.Spec
{
    public class GetFilteredExtracurricularSpec : BaseSpecification<Extracurricular>
    {

        public GetFilteredExtracurricularSpec(string name, long? sportId, long? audienceTypeId, Guid? placeId, int skip, int take)
        {
            ApplyCriteria(entity =>
                (string.IsNullOrEmpty(name) || entity.Name.Equals(name)) &&
                (!sportId.HasValue || entity.SportId.Equals(sportId.Value)) &&
                (!audienceTypeId.HasValue || entity.AudienceTypeId.Equals(audienceTypeId.Value)) &&
                (!placeId.HasValue || entity.PlaceId.Equals(placeId.Value)));


            AddInclude(b => b.AudienceType);
            AddInclude(b => b.Sport);
            AddInclude(b => b.Place);

            ApplyPaging(skip, take);
        }

    }
}
