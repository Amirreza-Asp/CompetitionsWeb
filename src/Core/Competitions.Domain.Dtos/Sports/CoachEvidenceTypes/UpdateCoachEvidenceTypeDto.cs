using Competitions.Domain.Dtos.Sports.SportTypes;
using Competitions.Domain.Entities.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Competitions.Domain.Dtos.Sports.CoachEvidenceTypes
{
    public class UpdateCoachEvidenceTypeDto : CreateCoachEvidenceTypeDto
    {
        public long Id { get; set; }

        public static UpdateCoachEvidenceTypeDto Create(CoachEvidenceType entity) =>
            new UpdateCoachEvidenceTypeDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description
            };
    }
}
