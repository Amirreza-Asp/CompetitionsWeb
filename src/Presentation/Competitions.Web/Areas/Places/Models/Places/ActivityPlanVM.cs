using Competitions.Domain.Dtos.Places.ActivityPlans;

namespace Competitions.Web.Areas.Places.Models.Places
{
	public class ActivityPlanVM
	{
		public ActivityPlanDto? ActivityPlan { get; set; }
		public Guid PlaceId { get; set; }
		public String? NewFile { get; set; }
	}
}
