using System.ComponentModel.DataAnnotations;

namespace Competitions.Domain.Dtos.Authentication.User
{
    public class LoginDto
    {
        [Required(ErrorMessage = "نام کاربری را وارد کنید")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "رمز عبور را وارد کنید")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
