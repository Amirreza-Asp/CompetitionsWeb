using Competitions.Domain.Entities.Notifications;

namespace Competitions.Domain.Entities.Notifications.Spec
{
    public class GetFilteredNotificationsSpec : BaseSpecification<Notification>
    {

        public GetFilteredNotificationsSpec(int skip, int take)
        {
            ApplyPaging(skip, take);
        }

    }
}
