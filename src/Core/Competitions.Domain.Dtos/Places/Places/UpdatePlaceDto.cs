using Competitions.Domain.Dtos.Places.Image;

namespace Competitions.Domain.Dtos.Places.Places
{
	public class UpdatePlaceDto : CreatePlaceDto
	{
		public Guid Id { get; set; }

		public IEnumerable<PlaceImageDto>? CurrentImages { get; set; }
	}
}
