using System.ComponentModel.DataAnnotations;

namespace Competitions.Domain.Dtos.Matches.Festivals
{
    public class CreateFestivalDto
    {
        [Required(ErrorMessage = "عنوان جشنواره را وارد کنید")]
        public string Title { get; set; }

        [Required(ErrorMessage = "تاریخ شروع جشنواره را وارد کنید")]
        public DateTime Start { get; set; }

        [Required(ErrorMessage = "تاریخ پایان جشنواره را وارد کنید")]
        public DateTime End { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }
    }
}
