using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Competitions.Domain.Dtos.Managment.Matches
{
    public class MatchFirstInfoDto
    {
        public bool ReadOnly { get; set; }
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "نام مسابقه را وارد کنید")]
        public String Name { get; set; }

        public bool Gender { get; set; }

        [Required(ErrorMessage = "سطح مسابقه را وارد کنید")]
        public String Level { get; set; }

        [Range(1 , int.MaxValue , ErrorMessage = "ظرفیت تعداد شرکت کنندگان را وارد کنید")]
        public int Capacity { get; set; } = 1;

        [Range(1 , int.MaxValue , ErrorMessage = "تعداد افراد هر تیم را وارد کنید")]
        public byte TeamCount { get; set; } = 1;

        public String? Description { get; set; }

        [Required(ErrorMessage = "تاریخ شروع ثبت نام را وارد کنید")]
        public String StartRegister { get; set; }

        [Required(ErrorMessage = "تاریخ پایان ثبت نام را وارد کنید")]
        public String EndRegister { get; set; }

        [Required(ErrorMessage = "تاریخ برگزاری مسابقه را وارد کنید")]
        public String StartPutOn { get; set; }

        [Required(ErrorMessage = "تاریخ اتمام مسابقه را وارد کنید")]
        public String EndPutOn { get; set; }

        public Guid? FestivalId { get; set; }

        [Required(ErrorMessage = "مکان برگزاری را مشخص کنید")]
        public Guid PlaceId { get; set; }

        [Required(ErrorMessage = "رشته ورزشی را وارد کنید")]
        public long SportId { get; set; }

        [Required(ErrorMessage = "مخاطبین مسابقه را وارد کنید")]
        public String Audience { get; set; }


        public IEnumerable<SelectListItem>? Festivals { get; set; }
        public IEnumerable<SelectListItem>? Places { get; set; }
        public IEnumerable<SelectListItem>? AudienceTypes { get; set; }


        public IEnumerable<SelectListItem> GetLevels ()
        {
            return new List<SelectListItem>
            {
                new SelectListItem(){Text = "درون دانشگاهی" , Value = "درون دانشگاهی"},
                new SelectListItem(){Text = "استانی" , Value = "استانی"},
                new SelectListItem(){Text = "منطقه ای" , Value = "منطقه ای"},
                new SelectListItem(){Text = "المپیاد" , Value = "المپیاد"},
                new SelectListItem(){Text = "کشوری" , Value = "کشوری"},
                new SelectListItem(){Text = "بین مناطق" , Value = "بین مناطق"},
            };
        }
        public IEnumerable<SelectListItem> GetGenders ()
        {
            return new List<SelectListItem>
            {
                new SelectListItem{Text ="آقا" , Value=false.ToString()},
                new SelectListItem{Text ="خانم" , Value=true.ToString()},
            };
        }
    }
}
