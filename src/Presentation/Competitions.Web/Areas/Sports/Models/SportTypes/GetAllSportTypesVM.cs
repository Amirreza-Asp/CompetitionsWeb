using Competitions.Domain.Entities.Sports;

namespace Competitions.Web.Areas.Sports.Models.SportTypes
{
	public class GetAllSportTypesVM
	{
		public IEnumerable<SportType> Entities { get; set; }
		public SportTypeFilter Filters { get; set; }
	}
}
