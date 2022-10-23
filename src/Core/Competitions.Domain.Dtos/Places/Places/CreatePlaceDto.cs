using Competitions.Domain.Dtos.Places.Supervisor;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Competitions.Domain.Dtos.Places.Places
{
    public class CreatePlaceDto
    {
        [Required(ErrorMessage = "عنوان را وارد کنید")]
        public String Title { get; set; }

        [Range(1 , long.MaxValue , ErrorMessage = "نوع مکان را وارد کنید")]
        public long PlaceTypeId { get; set; }

        [Required(ErrorMessage = "آدرس را وارد کنید")]
        public String Address { get; set; }

        [Range(1 , long.MaxValue , ErrorMessage = "متراژ را وارد کنید")]
        public long Meterage { get; set; }

        public Guid? ParentPlaceId { get; set; }

        public CreateSupervisorDto Supervisor { get; set; }

        [Required(ErrorMessage = "رشته های ورزشی را وارد کنید")]
        public String NewSports { get; set; }

        public IEnumerable<SelectListItem>? Places { get; set; }
        public IEnumerable<String>? NewImages { get; set; }
        public IEnumerable<SelectListItem>? Types { get; set; }
        public IEnumerable<SelectListItem>? Sports { get; set; }
    }
}
