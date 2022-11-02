using Microsoft.AspNetCore.Mvc.Rendering;

namespace Competitions.Web.Areas.Managment.Models.Extracurriculars
{
    public class ExtracurricularFilter
    {
        // Filter
        public String? Name { get; set; }
        public long? SportId { get; set; }
        public long? AudienceTypeId { get; set; }
        public Guid? PlaceId { get; set; }


        public IEnumerable<SelectListItem>? Sports { get; set; }
        public IEnumerable<SelectListItem>? Places { get; set; }
        public IEnumerable<SelectListItem>? AudienceTypes { get; set; }

        public bool IsEmpty () => String.IsNullOrWhiteSpace(Name) && !SportId.HasValue && !AudienceTypeId.HasValue && !PlaceId.HasValue;


        // Pagenation
        public int Take { get; set; } = 10;
        public int Skip { get; set; }
        public int Total { get; set; }
    }
}
