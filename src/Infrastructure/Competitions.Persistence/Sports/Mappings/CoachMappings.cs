using Competitions.Domain.Entities.Sports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Competitions.Persistence.Sports.Mappings
{
    public class CoachMappings : IEntityTypeConfiguration<Coach>
    {
        public void Configure ( EntityTypeBuilder<Coach> builder )
        {
            builder.HasKey(x => x.Id);

            builder.Property(b => b.Name);
            builder.Property(b => b.Family);
            builder.Property(b => b.Education);
            builder.Property(b => b.Description).IsRequired(false);

            builder.OwnsOne(b => b.PhoneNumber , b =>
            {
                b.Property(p => p.Value).HasColumnName("PhoneNumber").HasMaxLength(11);
            });
            builder.OwnsOne(b => b.NationalCode , b =>
            {
                b.Property(p => p.Value).HasColumnName("NationalCode").HasMaxLength(10);
            });

            builder.HasOne(b => b.Sport)
                .WithMany(b => b.Coaches)
                .HasForeignKey(b => b.SportId);

            builder.HasOne(b => b.CoachEvidenceType)
                .WithMany(b => b.Coaches)
                .HasForeignKey(b => b.CETId);
        }
    }
}
