using Microsoft.AspNetCore.Mvc.Rendering;

namespace Competitions.Web.Areas.Sports.Models.Coaches
{
    public class CoachFilter
    {
        //filters
        public String? Name { get; set; }
        public String? Family { get; set; }
        public String? Education { get; set; }
        public String? NationalCode { get; set; }
        public long? SportId { get; set; }
        public long? CETId { get; set; }


        public IEnumerable<SelectListItem>? Sports { get; set; }
        public IEnumerable<SelectListItem>? CETs { get; set; }


        // pagenation
        public int Take { get; set; } = 10;
        public int Skip { get; set; }
        public int Total { get; set; }


        public bool IsEmpty () => String.IsNullOrEmpty(Name) && String.IsNullOrEmpty(Family) &&
            String.IsNullOrEmpty(Education) && String.IsNullOrEmpty(NationalCode) && !SportId.HasValue && !CETId.HasValue;
    }
}
