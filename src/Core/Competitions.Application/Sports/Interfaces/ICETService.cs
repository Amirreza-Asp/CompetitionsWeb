using Microsoft.AspNetCore.Mvc.Rendering;

namespace Competitions.Application.Sports.Interfaces
{
    public interface ICETService
    {
        IEnumerable<SelectListItem> GetSelectedList ();
    }
}
