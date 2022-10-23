namespace Competitions.Web.Areas.Sports.Models.SportTypes
{
	public class SportTypeFilter
	{
		public int TotalCount { get; set; }
		public int Skip { get; set; }
		public int Take { get; set; } = 10;
	}
}
