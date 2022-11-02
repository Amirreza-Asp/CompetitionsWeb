namespace Competitions.Web.Areas.Managment.Models.Matches
{
    public class MatchFilter
    {
        // pagenation
        public int Skip { get; set; }
        public int Take { get; set; } = 10;
        public int Total { get; set; }
    }
}
