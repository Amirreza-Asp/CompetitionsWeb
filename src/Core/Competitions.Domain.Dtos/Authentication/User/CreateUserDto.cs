using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Competitions.Domain.Dtos.Authentication.User
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "نام شخص را وارد کنید")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "کد ملی را وارد کنید")]
        [MaxLength(10 , ErrorMessage = "کد ملی 10 رقمی است")]
        [MinLength(10 , ErrorMessage = "کد ملی 10 رقمی است")]
        public string NationalCode { get; set; }

        [Required(ErrorMessage = "سطح دسترسی را وارد کنید")]
        public Guid RoleId { get; set; }

        public IEnumerable<SelectListItem>? Positions { get; set; }

        public IEnumerable<SelectListItem>? Roles { get; set; }
    }
}