using Microsoft.AspNetCore.Mvc.Rendering;

namespace Competitions.Web.Areas.Sports.Models.Sports
{
    public class SportFilter
    {
        // filters
        public String? Name { get; set; }
        public DateTime?  CreateDate { get; set; }
        public long? SportTypeId { get; set; }

        public IEnumerable<SelectListItem>? Types { get; set; }


        // pagenation
        public int Take { get; set; } = 10;
        public int Skip { get; set; }
        public int Total { get; set; }


        public bool IsEmpty() => !SportTypeId.HasValue && String.IsNullOrEmpty(Name) && !CreateDate.HasValue;
    }
}
