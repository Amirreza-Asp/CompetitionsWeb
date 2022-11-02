namespace Competitions.Domain.Entities.Managment.Spec
{
    public class GetFilteredMatchSpec : BaseSpecification<Match>
    {

        public GetFilteredMatchSpec ( int skip , int take )
        {
            ApplyPaging(skip , take);

            AddInclude(u => u.Sport);
        }


    }
}
