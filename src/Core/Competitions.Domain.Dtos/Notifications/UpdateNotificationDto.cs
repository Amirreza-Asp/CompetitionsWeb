namespace Competitions.Domain.Dtos.Notifications
{
    public class UpdateNotificationDto : CreateNotificationDto
    {
        public long Id { get; set; }

        public IEnumerable<NotificaionImageDto>? CurrentImages { get; set; }
    }

    public class NotificaionImageDto
    {
        public String Name { get; set; }
        public long Id { get; set; }
    }
}
