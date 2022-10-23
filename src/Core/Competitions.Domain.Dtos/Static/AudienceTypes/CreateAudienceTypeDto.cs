using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Competitions.Domain.Dtos.Static.AudienceTypes
{
    public class CreateAudienceTypeDto
    {
        [Required(ErrorMessage = "عنوان مخاطب را وارد کنید")]
        public string Title { get; set; }

        public string? Description { get; set; }
    }
}
