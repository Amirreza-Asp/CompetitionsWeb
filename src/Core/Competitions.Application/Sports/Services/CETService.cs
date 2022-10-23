using Competitions.Application.Sports.Interfaces;
using Competitions.Domain.Entities.Sports;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Competitions.Application.Sports.Services
{
    public class CETService : ICETService
    {
        private readonly IRepository<CoachEvidenceType> _repo;

        public CETService ( IRepository<CoachEvidenceType> repo )
        {
            _repo = repo;
        }

        public IEnumerable<SelectListItem> GetSelectedList () =>
            _repo.GetAll<SelectListItem>(
                select: entity => new SelectListItem
                {
                    Text = entity.Title ,
                    Value = entity.Id.ToString()
                });
    }
}
