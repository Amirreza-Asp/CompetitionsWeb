using Competitions.Domain.Entities.Managment;

namespace Competitions.Web.Areas.Managment.Models.Festivals
{
    public class GetAllFestivalsVM
    {
        public IEnumerable<Festival> Entities { get; set; }
        public FestivalFilter Filters { get; set; }
    }
}
