using Microsoft.AspNetCore.Mvc.Rendering;

namespace Competitions.Web.Areas.Places.Models.Places
{
	public class MultipleSelect
	{
		public MultipleSelect ( string @for , string? values , IEnumerable<SelectListItem>? items )
		{
			For = @for;
			Values = values;
			Items = items;
		}

		public MultipleSelect ()
		{
		}

		public String For { get; set; }
		public String? Values { get; set; }
		public IEnumerable<SelectListItem>? Items { get; set; }
	}
}
