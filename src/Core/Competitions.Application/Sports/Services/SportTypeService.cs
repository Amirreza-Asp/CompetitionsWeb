using Competitions.Application.Sports.Interfaces;
using Competitions.Domain.Entities.Sports;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Competitions.Application.Sports.Services
{
    public class SportTypeService : ISportTypeService
    {
        private readonly IRepository<SportType> _repo;

        public SportTypeService(IRepository<SportType> repo)
        {
            _repo = repo;
        }

        public IEnumerable<SelectListItem> GetSelectedList()
        {
            return _repo.GetAll(select: entity => new SelectListItem
            {
                Text = entity.Title,
                Value = entity.Id.ToString()
            }); 
        }
    }
}
