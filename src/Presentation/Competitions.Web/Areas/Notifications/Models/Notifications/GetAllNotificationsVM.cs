using Competitions.Domain.Entities.Notifications;

namespace Competitions.Web.Areas.Managment.Models.Notifications
{
    public class GetAllNotificationsVM
    {
        public IEnumerable<Notification> Entities { get; set; }
        public NotificationFilter Filters { get; set; }
    }
}
