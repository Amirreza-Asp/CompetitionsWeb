using Competitions.Domain.Dtos.Sports.Sports;
using Competitions.Domain.Entities.Sports;
using Competitions.Web.Models;

namespace Competitions.Web.Areas.Sports.Models.Sports
{
    public class GetAllSportsVM
    {
        public IEnumerable<GetSportDto> Entities { get; set; }
        public SportFilter Filters { get; set; }
    }
}
