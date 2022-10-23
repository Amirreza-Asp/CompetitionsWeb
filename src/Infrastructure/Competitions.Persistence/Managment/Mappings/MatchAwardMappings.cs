using Competitions.Domain.Entities.Managment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Competitions.Persistence.Managment.Mappings
{
    public class MatchAwardMappings : IEntityTypeConfiguration<MatchAward>
    {
        public void Configure ( EntityTypeBuilder<MatchAward> builder )
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Score);
            builder.Property(b => b.Prize);

            builder.HasOne(b => b.Match)
                .WithMany(b => b.Awards)
                .HasForeignKey(b => b.MatchId);
        }
    }
}
