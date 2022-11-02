using System.ComponentModel.DataAnnotations;

namespace Competitions.Domain.Dtos.Notifications
{
    public class CreateNotificationDto
    {
        [Required(ErrorMessage = "عنوان اطلاعیه را وارد کنید")]
        public string Title { get; set; }

        [Required(ErrorMessage = "شرح اطلاعیه را وارد کنید")]
        public string Description { get; set; }

        public List<string>? NewImages { get; set; }
    }
}
