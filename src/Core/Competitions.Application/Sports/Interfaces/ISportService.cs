using Microsoft.AspNetCore.Mvc.Rendering;

namespace Competitions.Application.Sports.Interfaces
{
    public interface ISportService
    {
        IEnumerable<SelectListItem> GetSelectedList ();
    }
}
