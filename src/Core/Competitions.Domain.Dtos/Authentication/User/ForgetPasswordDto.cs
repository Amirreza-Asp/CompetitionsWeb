using System.ComponentModel.DataAnnotations;

namespace Competitions.Domain.Dtos.Authentication.User
{
    public class ForgetPasswordDto
    {
        [Required(ErrorMessage = "کد ملی را وارد کنید")]
        [MaxLength(10 , ErrorMessage = "کد ملی 10 رقمی است")]
        [MinLength(10 , ErrorMessage = "کد ملی 10 رقمی است")]
        public string NationalCode { get; set; }

        public int SecretCode { get; set; }
    }
}
