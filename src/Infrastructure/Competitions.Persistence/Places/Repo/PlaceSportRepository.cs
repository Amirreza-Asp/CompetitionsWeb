using Competitions.Domain.Entities.Places;
using Competitions.Domain.Entities.Places.Repo;
using Competitions.Persistence.Data;

namespace Competitions.Persistence.Places.Repo
{
	public class PlaceSportRepository : IPlaceSportRepository
	{
		private readonly ApplicationDbContext _context;

		public PlaceSportRepository ( ApplicationDbContext context )
		{
			_context = context;
		}

		public void Create ( PlaceSports entity )
		{
			_context.Add(entity);
		}

		public void Remove ( PlaceSports entity )
		{
			_context.Remove(entity);
		}
	}
}
