using System.ComponentModel.DataAnnotations;

namespace Competitions.Domain.Dtos.Extracurriculars
{
    public class ExtracurricularUserInformationDto
    {
        public Guid EtcId { get; set; }

        [Required(ErrorMessage = "لطفا نام را وارد کنید")]
        public String Name { get; set; }

        [Required(ErrorMessage = "لطفا نام خانوادگی را وارد کنید")]
        public String Family { get; set; }

        [Required(ErrorMessage = "لطفا کدملی را وارد کنید")]
        [MaxLength(10, ErrorMessage = "کد ملی باید 10 رقم باشد")]
        [MinLength(10, ErrorMessage = "کد ملی باید 10 رقم باشد")]
        public String NationalCode { get; set; }

        public bool Gender { get; set; }

        [Required(ErrorMessage = "لطفا شماره تلفن را وارد کنید")]
        [MaxLength(13, ErrorMessage = "شماره تلفن بین 11 تا 13 رقم است")]
        [MinLength(11, ErrorMessage = "شماره تلفن بین 11 تا 13 رقم است")]
        public String PhoneNumber { get; set; }

        [Required(ErrorMessage = "لطفا نسبیت را وارد کنید")]
        public String Relativity { get; set; }

        public bool Insurance { get; set; }
    }
}
