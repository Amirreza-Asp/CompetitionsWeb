using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Competitions.Domain.Dtos.Extracurriculars
{
    public class CreateExtracurricularDto
    {
        [Required(ErrorMessage = "نام را وارد کنید")]
        public string Name { get; set; }

        [Required(ErrorMessage = "رشته ورزشی را وارد کنید")]
        public long SportId { get; set; }

        [Required(ErrorMessage = "مکان برگزاری را وارد کنید")]
        public Guid PlaceId { get; set; }

        [Required(ErrorMessage = "نوع مخاطبین را مشخص کنید")]
        public long AudienceTypeId { get; set; }

        [Required(ErrorMessage = "ظرفیت دوره را مشخص کنید")]
        [Range(1, int.MaxValue, ErrorMessage = "ظرفیت دوره باید بیشتر از صفر باشد")]
        public int Capacity { get; set; } = 1;

        [Required(ErrorMessage = "حداقل تعداد افراد برای برگزاری را وارد کنید")]
        [Range(1, int.MaxValue, ErrorMessage = "حداقل تعداد افراد برای برگزاری باید بیشتر از صفر باشد")]
        public int MinimumPlacements { get; set; } = 1;

        [Required(ErrorMessage = "تاریخ شروع دوره را وارد کنید")]
        public DateTime StartPutOn { get; set; }

        [Required(ErrorMessage = "تاریخ پایان  دوره را وارد کنید")]
        public DateTime EndPutOn { get; set; }

        [Required(ErrorMessage = "جنسیت شرکت کنندگان را مشخص کنید")]
        public bool Gender { get; set; }

        [Required(ErrorMessage = "تاریخ شروع ثبت نام را وارد کنید")]
        public DateTime StartRegister { get; set; }

        [Required(ErrorMessage = "تاریخ پایان ثبت نام را وارد کنید")]
        public DateTime EndRegister { get; set; }

        [Required(ErrorMessage = "توضیحات را وارد کنید")]
        public string Description { get; set; }

        [Required(ErrorMessage = "روز و زمان برگزاری را مشخص کنید")]
        public string Times { get; set; }

        [Required(ErrorMessage = "نام مربی را وارد کنید")]
        public long CoachId { get; set; }

        public bool IsFree { get; set; }


        public IEnumerable<SelectListItem>? AudienceTypes { get; set; }
        public IEnumerable<SelectListItem>? Places { get; set; }

        public List<SelectListItem> GetGenders() => new List<SelectListItem>
        {
            new SelectListItem{Text = "مرد" , Value = "false"},
            new SelectListItem{Text = "زن" , Value = "true"}
        };
    }
}
