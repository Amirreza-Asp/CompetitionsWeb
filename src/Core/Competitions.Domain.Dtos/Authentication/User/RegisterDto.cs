using System.ComponentModel.DataAnnotations;

namespace Competitions.Domain.Dtos.Authentication.User
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "کد ملی را وارد کنید")]
        [MaxLength(10, ErrorMessage = "کد ملی 10 رقمی است")]
        [MinLength(10, ErrorMessage = "کد ملی 10 رقمی است")]
        public string NationalCode { get; set; }

        [Required(ErrorMessage = "شماره دانشجویی را وارد کنید")]
        [MaxLength(10, ErrorMessage = "شماره دانشجویی 9 یا 10 رقم است")]
        [MinLength(9, ErrorMessage = "شماره دانشجویی 9 یا 10 رقم است")]
        public String StudentNumber { get; set; }

        [Required(ErrorMessage = "رمز عبور را وارد کنید")]
        [MinLength(8, ErrorMessage = "رمز عبور حداقل 8 کاراکتر است")]
        [MaxLength(12, ErrorMessage = "رمز عبور حداکثر 12 کاراکتر است")]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        [Required(ErrorMessage = "تکرار رمز عبور را وارد کنید")]
        [MinLength(8, ErrorMessage = "تکرار رمز عبور حداقل 8 کاراکتر است")]
        [MaxLength(12, ErrorMessage = "تکرار رمز عبور حداکثر 12 کاراکتر است")]
        [Compare(nameof(Password), ErrorMessage = "تکرار رمز عبور را رمز عبور مطابقت ندارد")]
        public String ConfirmPassword { get; set; }

    }
}
