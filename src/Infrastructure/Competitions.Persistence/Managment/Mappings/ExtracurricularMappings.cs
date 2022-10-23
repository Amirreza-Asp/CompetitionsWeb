using Competitions.Domain.Entities.Managment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Competitions.Persistence.Managment.Mappings
{
    public class ExtracurricularMappings : IEntityTypeConfiguration<Extracurricular>
    {
        public void Configure ( EntityTypeBuilder<Extracurricular> builder )
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedNever();

            builder.Property(b => b.Name);
            builder.Property(b => b.Capacity);
            builder.Property(b => b.Gender);
            builder.Property(b => b.Description);
            builder.Property(b => b.MinimumPlacements);


            builder.OwnsOne(b => b.PutOn , b =>
            {
                b.Property(p => p.From).HasColumnName("StartPutOn").IsRequired();
                b.Property(p => p.To).HasColumnName("EndPutOn").IsRequired();
            });


            builder.OwnsOne(b => b.Register , b =>
            {
                b.Property(p => p.From).HasColumnName("StartRegister").IsRequired();
                b.Property(p => p.To).HasColumnName("EndRegister").IsRequired();
            });

            builder.HasOne(b => b.Place)
                .WithMany(b => b.Extracurriculars)
                .HasForeignKey(b => b.PlaceId);

            builder.HasOne(b => b.Coach)
                .WithMany(b => b.Extracurriculars)
                .HasForeignKey(b => b.CoachId);

            builder.HasOne(b => b.AudienceType)
                .WithMany(b => b.Extracurriculars)
                .HasForeignKey(b => b.AudienceTypeId);

            builder.HasOne(b => b.Sport)
                .WithMany(b => b.Extracurriculars)
                .HasForeignKey(b => b.SportId);
        }
    }
}
