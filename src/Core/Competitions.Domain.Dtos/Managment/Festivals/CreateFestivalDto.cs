using System.ComponentModel.DataAnnotations;

namespace Competitions.Domain.Dtos.Managment.Festivals
{
    public class CreateFestivalDto
    {
        [Required(ErrorMessage = "عنوان جشنواره را وارد کنید")]
        public String Title { get; set; }

        [Required(ErrorMessage = "تاریخ شروع جشنواره را وارد کنید")]
        public DateTime Start { get; set; }

        [Required(ErrorMessage = "تاریخ پایان جشنواره را وارد کنید")]
        public DateTime End { get; set; }

        public String? Description { get; set; }

        public String? Image { get; set; }
    }
}
