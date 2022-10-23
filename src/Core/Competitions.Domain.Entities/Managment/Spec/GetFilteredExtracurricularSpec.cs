namespace Competitions.Domain.Entities.Managment.Spec
{
    public class GetFilteredExtracurricularSpec : BaseSpecification<Extracurricular>
    {

        public GetFilteredExtracurricularSpec ( String name , long? sportId , long? audienceTypeId , Guid? placeId , int skip , int take )
        {
            ApplyCriteria(entity =>
                ( String.IsNullOrEmpty(name) || entity.Name.Equals(name) ) &&
                ( !sportId.HasValue || entity.SportId.Equals(sportId.Value) ) &&
                ( !audienceTypeId.HasValue || entity.AudienceTypeId.Equals(audienceTypeId.Value) ) &&
                ( !placeId.HasValue || entity.PlaceId.Equals(placeId.Value) ));


            AddInclude(b => b.AudienceType);
            AddInclude(b => b.Sport);
            AddInclude(b => b.Place);

            ApplyPaging(skip , take);
        }

    }
}
