using Competitions.Domain.Entities.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Competitions.Domain.Dtos.Sports.SportTypes
{
    public class UpdateSportTypeDto : CreateSportTypeDto
    {
        public long Id { get; set; }


        public static UpdateSportTypeDto Create(SportType entity) =>
            new UpdateSportTypeDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description
            };

    }
}
