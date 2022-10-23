using Competitions.Domain.Entities.Managment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Competitions.Persistence.Managment.Mappings
{
    public class TeamMappings : IEntityTypeConfiguration<Team>
    {
        public void Configure ( EntityTypeBuilder<Team> builder )
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedNever();

            builder.Property(b => b.CreateDate);

            builder.HasOne(b => b.Match)
                .WithMany(b => b.Teams)
                .HasForeignKey(b => b.MatchId);


        }
    }
}
