namespace Competitions.Domain.Entities.Places.Spec
{
    public class GetFilteredPlacesSpec : BaseSpecification<Place>
    {
        public GetFilteredPlacesSpec ( long? placeTypeId , string? title , long? sportId , int skip , int take )
        {
            ApplyCriteria(entity =>
                ( String.IsNullOrEmpty(title) || entity.Title.Equals(title) ) &&
                ( !placeTypeId.HasValue || entity.PlaceTypeId.Equals(placeTypeId.Value) ) &&
                ( !sportId.HasValue || entity.Sports.Any(u => u.SportId == sportId.Value) ));

            AddInclude(source => source.Supervisor);
            AddInclude(source => source.Images);

            ApplyPaging(skip , take);
        }
    }
}
