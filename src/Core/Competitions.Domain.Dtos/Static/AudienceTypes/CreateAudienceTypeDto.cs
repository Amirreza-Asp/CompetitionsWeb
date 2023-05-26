using System.ComponentModel.DataAnnotations;

namespace Competitions.Domain.Dtos.Static.AudienceTypes
{
    public class CreateAudienceTypeDto
    {
        [Required(ErrorMessage = "عنوان مخاطب را وارد کنید")]
        public string Title { get; set; }

        public bool IsNeedInformation { get; set; }

        public string? Description { get; set; }
    }
}
