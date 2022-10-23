using Competitions.Domain.Entities.Sports;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Competitions.Domain.Dtos.Sports.Coaches
{
    public class CreateCoachDto
    {
        [Required(ErrorMessage = "نام را وارد کنید")]
        public String Name { get; set; }

        [Required(ErrorMessage = "نام خانوادگی را وارد کنید")]
        public String Family { get; set; }

        [Required(ErrorMessage = "کد ملی را وارد کنید")]
        [MaxLength(10 , ErrorMessage = "کد ملی 10 رقم است")]
        [MinLength(10 , ErrorMessage = "کد ملی 10 رقم است")]
        public String NationalCode { get; set; }

        [Required(ErrorMessage = "شماره موبایل را وارد کنید")]
        [MaxLength(11 , ErrorMessage = "شماره موبایل 11 رقم است")]
        [MinLength(11 , ErrorMessage = "شماره موبایل 11 رقم است")]
        public String PhoneNumber { get; set; }

        [Required(ErrorMessage = "تحصیلات را وارد کنید")]
        public String Education { get; set; }

        public String? Description { get; set; }

        [Range(1 , long.MaxValue , ErrorMessage = "رشته ورزشی را انتخاب کنید")]
        public long SportId { get; set; }

        [Range(1 , long.MaxValue , ErrorMessage = "مدرک ورزشی را انتخاب کنید")]
        public long CETId { get; set; }



        public IEnumerable<SelectListItem>? CETs { get; set; }
        public IEnumerable<SelectListItem>? Sports { get; set; }


        public Coach ToEntity () => new Coach(Name , Family , PhoneNumber , NationalCode , CETId , SportId , Description , Education);
    }
}
