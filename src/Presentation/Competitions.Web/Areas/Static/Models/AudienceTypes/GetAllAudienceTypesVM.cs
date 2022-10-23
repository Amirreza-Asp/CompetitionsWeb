using Competitions.Domain.Entities.Static;

namespace Competitions.Web.Models.AudienceTypes
{
    public class GetAllAudienceTypesVM
	{
		public IEnumerable<AudienceType> Entities { get; set; }

		public Pagenation Pagenation { get; set; }
	}
}
