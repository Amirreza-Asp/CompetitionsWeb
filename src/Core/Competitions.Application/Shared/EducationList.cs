using Microsoft.AspNetCore.Mvc.Rendering;

namespace Competitions.Application.Shared
{
    public class EducationList
    {
        private static readonly List<String> _educations = new List<string>
        {
            "زیر دیپلم",
            "دیپلم",
            "کاردانی",
            "کارشناسی",
            "کارشناسی ارشد",
            "دکترا",
        };

        public static IEnumerable<SelectListItem> GetSelectedItems () =>
            _educations.Select(edu => new SelectListItem
            {
                Text = edu ,
                Value = edu
            });
    }
}
