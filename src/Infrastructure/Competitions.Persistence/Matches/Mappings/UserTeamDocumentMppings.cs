using Competitions.Domain.Entities.Managment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Competitions.Persistence.Managment.Mappings
{
    public class UserTeamDocumentMppings : IEntityTypeConfiguration<UserTeamDocument>
    {
        public void Configure ( EntityTypeBuilder<UserTeamDocument> builder )
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.CreateDate);
            builder.OwnsOne(b => b.File , b =>
            {
                b.Property(p => p.Name).HasColumnName("File").IsUnicode(false).HasMaxLength(50);
            });

            builder.HasOne(b => b.UserTeam)
                .WithMany(b => b.Documents)
                .HasForeignKey(b => b.UserTeamId);
        }
    }
}
