namespace Competitions.Domain.Entities.Sports.Spec
{
    public class GetFilteredCoachSpec : BaseSpecification<Coach>
    {
        public GetFilteredCoachSpec ( string? name , string? family , string? education , string? nationalCode , long? sportId , long? cETId , int skip , int take )
        {
            ApplyCriteria(entity =>
                ( String.IsNullOrEmpty(name) || entity.Name.Equals(name) ) &&
                ( String.IsNullOrEmpty(family) || entity.Family.Equals(family) ) &&
                ( String.IsNullOrEmpty(education) || entity.Education.Equals(education) ) &&
                ( String.IsNullOrEmpty(nationalCode) || entity.NationalCode.Value.Equals(nationalCode) ) &&
                ( !sportId.HasValue || entity.SportId.Equals(sportId.Value) ) &&
                ( !cETId.HasValue || entity.CETId.Equals(cETId.Value) ));

            AddInclude(source => source.Sport);

            ApplyPaging(skip , take);
        }
    }
}
