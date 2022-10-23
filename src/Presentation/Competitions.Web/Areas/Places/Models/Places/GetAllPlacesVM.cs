using Competitions.Domain.Dtos.Places.Places;

namespace Competitions.Web.Areas.Places.Models.Places
{
    public class GetAllPlacesVM
    {
        public IEnumerable<GetPlaceDto> Places { get; set; }
        public PlaceFilter Filters { get; set; }
    }
}
