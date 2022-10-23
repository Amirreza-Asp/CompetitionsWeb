using Competitions.Domain.Entities.Places;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Competitions.Persistence.Places.Mappings
{
    public class PlaceImageMappings : IEntityTypeConfiguration<PlaceImages>
    {
        public void Configure ( EntityTypeBuilder<PlaceImages> builder )
        {
            builder.HasKey(b => b.Id);

            builder.OwnsOne(b => b.Image , b =>
            {
                b.Property(u => u.Name).HasColumnName("Image").IsUnicode(false).HasMaxLength(50);
            });

            builder.HasOne(b => b.Place)
                .WithMany(b => b.Images)
                .HasForeignKey(b => b.PlaceId);
        }
    }
}
