using Competitions.Domain.Entities.Static;

namespace Competitions.Domain.Dtos.Static.AudienceTypes
{
    public class UpdateAudienceTypeDto : CreateAudienceTypeDto
    {
        public long Id { get; set; }

        public static UpdateAudienceTypeDto Create(AudienceType entity) =>
            new UpdateAudienceTypeDto
            {
                Id = entity.Id,
                Title = entity.Title,
                IsNeedInformation = entity.IsNeedInformation,
                Description = entity.Description
            };
    }
}
