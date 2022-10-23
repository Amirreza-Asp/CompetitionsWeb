using Competitions.Domain.Dtos.Sports.SportTypes;
using Competitions.Domain.Entities.Sports;
using Competitions.Domain.Entities.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Competitions.Domain.Dtos.Static.Evidences
{
    public class UpdateEvidenceDto : CreateEvidenceDto
    {
        public long Id { get; set; }

        public static UpdateEvidenceDto Create(Evidence entity) =>
            new UpdateEvidenceDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description
            };
    }
}
