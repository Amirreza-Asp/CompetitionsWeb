using Competitions.Domain.Entities.Managment;

namespace Competitions.Web.Models.Notifications
{
    public class NotifDataVM
    {
        public IEnumerable<Notification> Notifications { get; set; }
        public Pagenation Pagenation { get; set; }
    }
}
