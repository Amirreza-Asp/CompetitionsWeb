using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Competitions.Domain.Dtos.Authentication.User
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "نام شخص را وارد کنید")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "کد ملی را وارد کنید")]
        [MaxLength(10, ErrorMessage = "کد ملی 10 رقمی است")]
        [MinLength(10, ErrorMessage = "کد ملی 10 رقمی است")]
        public string NationalCode { get; set; }

        [Required(ErrorMessage = "شماره موبایل را وارد کنید")]
        [MaxLength(13, ErrorMessage = "شماره موبایل نمیتواند بیشتر از 13 رقم باشد")]
        [MinLength(10, ErrorMessage = "شماره موبایل نمیتواند کمتر از 10 رقم باشد")]
        public String PhoneNumber { get; set; }

        [Required(ErrorMessage = "سطح دسترسی را وارد کنید")]
        public Guid RoleId { get; set; }

        public IEnumerable<SelectListItem>? Positions { get; set; }

        public IEnumerable<SelectListItem>? Roles { get; set; }
    }
}