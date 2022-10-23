namespace Competitions.Web.Areas.Managment.Models.Notifications
{
    public class NotificationFilter
    {
        // Pagenation
        public int Skip { get; set; }
        public int Take { get; set; } = 10;
        public int Total { get; set; }
    }
}
