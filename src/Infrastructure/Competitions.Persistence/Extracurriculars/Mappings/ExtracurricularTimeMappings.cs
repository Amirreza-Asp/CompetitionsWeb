using Competitions.Domain.Entities.Extracurriculars;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Competitions.Persistence.Extracurriculars.Mappings
{
    public class ExtracurricularTimeMappings : IEntityTypeConfiguration<ExtracurricularTime>
    {
        public void Configure(EntityTypeBuilder<ExtracurricularTime> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Day);
            builder.OwnsOne(b => b.Time, b =>
            {
                b.Property(p => p.Hour).HasColumnName("Hour").IsRequired();
                b.Property(p => p.Min).HasColumnName("Min").IsRequired();
            });
        }
    }
}
