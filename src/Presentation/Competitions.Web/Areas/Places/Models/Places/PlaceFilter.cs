namespace Competitions.Web.Areas.Places.Models.Places
{
    public class PlaceFilter
    {
        // Filters
        public long? PlaceTypeId { get; set; }
        public String? Title { get; set; }
        public long? SportId { get; set; }

        public bool IsEmpty () => !PlaceTypeId.HasValue && String.IsNullOrEmpty(Title) && !SportId.HasValue;

        // Pagenation
        public int Skip { get; set; }
        public int Take { get; set; } = 10;
        public int Total { get; set; }
    }
}
