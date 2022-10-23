using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Competitions.Application.Sports.Interfaces
{
    public interface ISportTypeService
    {
        IEnumerable<SelectListItem> GetSelectedList();
    }
}
