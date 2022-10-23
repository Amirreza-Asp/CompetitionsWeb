using Competitions.Domain.Entities.Static;

namespace Competitions.Web.Models.Evidences
{
    public class GetAllEvidenceVM
	{
		public IEnumerable<Evidence> Entities { get; set; }
		public Pagenation Pagenation { get; set; }
	}
}
