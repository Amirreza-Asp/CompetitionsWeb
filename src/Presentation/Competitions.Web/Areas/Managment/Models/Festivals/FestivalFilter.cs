namespace Competitions.Web.Areas.Managment.Models.Festivals
{
    public class FestivalFilter
    {
        // pagenation
        public int Skip { get; set; }
        public int Take { get; set; } = 10;
        public int Total { get; set; }
    }
}
