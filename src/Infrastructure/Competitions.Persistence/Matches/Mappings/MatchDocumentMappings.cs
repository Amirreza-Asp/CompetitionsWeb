using Competitions.Domain.Entities.Managment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Competitions.Persistence.Managment.Mappings
{
    public class MatchDocumentMappings : IEntityTypeConfiguration<MatchDocument>
    {
        public void Configure ( EntityTypeBuilder<MatchDocument> builder )
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Type);

            builder.HasOne(b => b.Match)
                .WithMany(b => b.Documents)
                .HasForeignKey(b => b.MatchId);

            builder.HasOne(b => b.Evidence)
                .WithMany(b => b.MatchDocuments)
                .HasForeignKey(b => b.EvidenceId);
        }
    }
}
