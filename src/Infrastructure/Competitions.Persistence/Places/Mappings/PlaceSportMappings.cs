using Competitions.Domain.Entities.Places;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Competitions.Persistence.Places.Mappings
{
    public class PlaceSportMappings : IEntityTypeConfiguration<PlaceSports>
    {
        public void Configure ( EntityTypeBuilder<PlaceSports> builder )
        {
            builder.HasKey(b => new { b.PlaceId , b.SportId });

            builder.HasOne(b => b.Sport)
                .WithMany(b => b.Places)
                .HasForeignKey(b => b.SportId);

            builder.HasOne(b => b.Place)
                .WithMany(b => b.Sports)
                .HasForeignKey(b => b.PlaceId);
        }
    }
}
