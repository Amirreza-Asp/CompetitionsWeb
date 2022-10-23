using Competitions.Domain.Entities.Places;
using Competitions.Web.Models;

namespace Competitions.Web.Areas.Places.Models.PlaceTypes
{
	public class GetAllPlaceTypesVM
	{
		public IEnumerable<PlaceType> Entities { get; set; }
		public Pagenation Pagenation { get; set; }
	}
}
