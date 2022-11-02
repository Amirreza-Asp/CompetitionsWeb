using Competitions.Domain.Entities.Managment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Competitions.Persistence.Managment.Mappings
{
    public class FestivalMappings : IEntityTypeConfiguration<Festival>
    {
        public void Configure ( EntityTypeBuilder<Festival> builder )
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedNever();

            builder.Property(b => b.Description).IsRequired(false);
            builder.Property(b => b.Title);

            builder.OwnsOne(b => b.Duration , b =>
            {
                b.Property(u => u.From).HasColumnName("Start");
                b.Property(u => u.To).HasColumnName("End");
            });

            builder.OwnsOne(b => b.Image , b =>
            {
                b.Property(u => u.Name).HasColumnName("Image").IsUnicode(false).HasMaxLength(50);
            });
        }
    }
}
