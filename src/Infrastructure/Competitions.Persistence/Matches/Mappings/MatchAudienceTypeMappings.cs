using Competitions.Domain.Entities.Managment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Competitions.Persistence.Managment.Mappings
{
    public class MatchAudienceTypeMappings : IEntityTypeConfiguration<MatchAudienceType>
    {
        public void Configure ( EntityTypeBuilder<MatchAudienceType> builder )
        {
            builder.HasKey(b => new { b.MatchId , b.AudienceTypeId });

            builder.HasOne(b => b.Match)
                .WithMany(b => b.AudienceTypes)
                .HasForeignKey(b => b.MatchId);

            builder.HasOne(b => b.AudienceType)
                .WithMany(b => b.Matchs)
                .HasForeignKey(b => b.AudienceTypeId);
        }
    }
}
