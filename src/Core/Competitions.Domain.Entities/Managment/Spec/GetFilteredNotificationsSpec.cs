namespace Competitions.Domain.Entities.Managment.Spec
{
    public class GetFilteredNotificationsSpec : BaseSpecification<Notification>
    {

        public GetFilteredNotificationsSpec ( int skip , int take )
        {
            ApplyPaging(skip , take);
        }

    }
}
