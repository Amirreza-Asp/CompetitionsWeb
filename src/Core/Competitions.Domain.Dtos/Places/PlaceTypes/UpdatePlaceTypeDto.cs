using Competitions.Domain.Dtos.Sports.SportTypes;
using Competitions.Domain.Entities.Places;
using Competitions.Domain.Entities.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Competitions.Domain.Dtos.Places.PlaceTypes
{
	public class UpdatePlaceTypeDto : CreatePlaceTypeDto
	{
		public long Id { get; set; }


        public static UpdatePlaceTypeDto Create(PlaceType entity) =>
            new UpdatePlaceTypeDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description
            };
    }
}
