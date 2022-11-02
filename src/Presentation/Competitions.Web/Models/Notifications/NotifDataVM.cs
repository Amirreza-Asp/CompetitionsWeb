using Competitions.Domain.Entities.Notifications;

namespace Competitions.Web.Models.Notifications
{
    public class NotifDataVM
    {
        public IEnumerable<Notification> Notifications { get; set; }
        public Pagenation Pagenation { get; set; }
    }
}
