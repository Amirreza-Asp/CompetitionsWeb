using System.ComponentModel.DataAnnotations;

namespace Competitions.Domain.Dtos.Authentication.User
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "رمز عبور را وارد کنید")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required(ErrorMessage = "تکرار رمز عبور را وارد کنید")]
        [Compare(nameof(Password) , ErrorMessage = "تکرار رمز عبور با رمز عبور شباهت ندارد")]
        public string ConfirmPassowrd { get; set; }
    }
}
