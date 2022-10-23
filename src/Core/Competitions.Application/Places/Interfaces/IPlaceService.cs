using Competitions.Domain.Dtos.Places.Places;
using Microsoft.AspNetCore.Http;

namespace Competitions.Application.Places.Interfaces
{
	public interface IPlaceService
	{
		Task CreateAsync ( CreatePlaceDto command , IFormFileCollection files );
		Task UpdateAsync ( UpdatePlaceDto command , IFormFileCollection files );
	}
}
