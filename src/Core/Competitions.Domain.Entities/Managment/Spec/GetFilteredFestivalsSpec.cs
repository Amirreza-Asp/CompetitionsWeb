namespace Competitions.Domain.Entities.Managment.Spec
{
    public class GetFilteredFestivalsSpec : BaseSpecification<Festival>
    {

        public GetFilteredFestivalsSpec ( int skip , int take )
        {
            ApplyPaging(skip , take);
        }

    }
}
