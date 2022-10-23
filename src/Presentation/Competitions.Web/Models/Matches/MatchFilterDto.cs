using Microsoft.AspNetCore.Mvc.Rendering;

namespace Competitions.Web.Models.Matches
{
    public class MatchFilterDto
    {
        public String? Level { get; set; } = "درون دانشگاهی";
        public DateTime? MatchDate { get; set; }



        public IEnumerable<SelectListItem> GetLevels ()
        {
            return new List<SelectListItem>
            {
                new SelectListItem(){Text = "درون دانشگاهی" , Value = "درون دانشگاهی"},
                new SelectListItem(){Text = "استانی" , Value = "استانی"},
                new SelectListItem(){Text = "منطقه ای" , Value = "منطقه ای"},
                new SelectListItem(){Text = "المپیاد" , Value = "المپیاد"},
                new SelectListItem(){Text = "کشوری" , Value = "کشوری"},
                new SelectListItem(){Text = "بین مناطق" , Value = "بین مناطق"},
            };
        }


        // pagenation
        public int Skip { get; set; }
        public int Take { get; set; } = 10;
        public int Total { get; set; }
    }
}
