using Microsoft.AspNetCore.Mvc.Rendering;

namespace Competitions.Web.Areas.Authentication.Models.Users
{
    public class UserFilter
    {
        // Filters
        public String Name { get; set; }
        public String Family { get; set; }
        public Guid? RoleId { get; set; }
        public String NationalCode { get; set; }

        public IEnumerable<SelectListItem>? Roles { get; set; }


        public bool IsEmpty () => String.IsNullOrEmpty(Name) && String.IsNullOrEmpty(Family) && !RoleId.HasValue && String.IsNullOrEmpty(NationalCode);

        // Pagenation
        public int Take { get; set; } = 10;
        public int Skip { get; set; }
        public int Total { get; set; }
    }
}
