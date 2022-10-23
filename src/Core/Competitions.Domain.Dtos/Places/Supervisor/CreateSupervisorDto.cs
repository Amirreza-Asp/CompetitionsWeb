using System.ComponentModel.DataAnnotations;

namespace Competitions.Domain.Dtos.Places.Supervisor
{
    public class CreateSupervisorDto
    {
        [Required(ErrorMessage = "شماره تماس سرپرست را وارد کنید")]
        [MaxLength(11 , ErrorMessage = "شماره تماس 11 رقمی است")]
        [MinLength(11 , ErrorMessage = "شماره تماس 11 رقمی است")]
        public String PhoneNumber { get; set; }

        [Required(ErrorMessage = "نام سرپرست را وارد کنید ")]
        public String Name { get; set; }
    }
}
