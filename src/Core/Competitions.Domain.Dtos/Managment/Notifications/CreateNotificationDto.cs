using System.ComponentModel.DataAnnotations;

namespace Competitions.Domain.Dtos.Managment.Notifications
{
    public class CreateNotificationDto
    {
        [Required(ErrorMessage = "عنوان اطلاعیه را وارد کنید")]
        public String Title { get; set; }

        [Required(ErrorMessage = "شرح اطلاعیه را وارد کنید")]
        public String Description { get; set; }

        public String? Image { get; set; }
    }
}
