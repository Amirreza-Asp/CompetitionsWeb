namespace Competitions.Domain.Entities.Sports.Spec
{
    public class GetFilteredSportsSpec : BaseSpecification<Sport>
    {
        public GetFilteredSportsSpec(String? name, DateTime? createDate, long? sportTypeId, int skip, int take)
        {
            ApplyCriteria(
                entity => (String.IsNullOrEmpty(name) || entity.Name.Equals(name)) &&
                                (!createDate.HasValue || entity.CreateDate.Date.Equals(createDate.Value.Date)) &&
                                (!sportTypeId.HasValue || entity.SportTypeId.Equals(sportTypeId.Value)));

            ApplyPaging(skip, take);
        }
    }
}
