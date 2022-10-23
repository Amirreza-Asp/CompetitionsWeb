using Competitions.Domain.Entities.Sports;
using Competitions.Web.Models;

namespace Competitions.Web.Areas.Sports.Models.CoachEvidenceTypes
{
	public class GetAllCETVM
	{
		public IEnumerable<CoachEvidenceType> Entities { get; set; }
		public Pagenation Pagenation { get; set; }
	}
}
