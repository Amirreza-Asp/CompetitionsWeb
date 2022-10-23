using Competitions.Application.Sports.Interfaces;
using Competitions.Domain.Entities.Sports;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Competitions.Application.Sports.Services
{
    public class SportService : ISportService
    {
        private readonly IRepository<Sport> _repo;

        public SportService ( IRepository<Sport> repo )
        {
            _repo = repo;
        }

        public IEnumerable<SelectListItem> GetSelectedList () =>
            _repo.GetAll<SelectListItem>(
                select: entity => new SelectListItem
                {
                    Text = entity.Name ,
                    Value = entity.Id.ToString()
                });
    }
}
