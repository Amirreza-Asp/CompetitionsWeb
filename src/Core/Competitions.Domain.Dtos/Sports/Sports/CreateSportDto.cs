using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Competitions.Domain.Dtos.Sports.Sports
{
    public class CreateSportDto
    {
        [Required(ErrorMessage ="لطفا نام رشته ورزشی را وارد کنید")]
        public String Name { get; set; }

        public String? Description { get; set; }

        public String? Image { get; set; }

        [Range(1 , long.MaxValue , ErrorMessage = "نوع رشته را وارد کنید")]
        public long SportTypeId { get; set; }

        public IEnumerable<SelectListItem>? SportTypes { get; set; }
    }
}
