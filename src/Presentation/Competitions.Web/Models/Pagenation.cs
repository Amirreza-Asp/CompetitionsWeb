namespace Competitions.Web.Models
{
    public class Pagenation
    {
        public Pagenation(int skip, int take, int total)
        {
            Skip = skip;
            Take = take;
            Total = total;
        }

        public Pagenation()
        {
            Skip = 0;
            Take = 50;
            Total = 0;
        }

        public int Skip { get; set; }
        public int Take { get; set; }
        public int Total { get; set; }
    }
}
