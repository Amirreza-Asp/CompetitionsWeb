using Competitions.Domain.Entities.Places;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Competitions.Persistence.Places.Mappings
{
    public class PlaceMappings : IEntityTypeConfiguration<Place>
    {
        public void Configure ( EntityTypeBuilder<Place> builder )
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedNever();

            builder.Property(b => b.Title);
            builder.Property(b => b.Address);
            builder.Property(b => b.Meterage);

            // Place type
            builder.HasOne(b => b.PlaceType)
                .WithMany(b => b.Places)
                .HasForeignKey(b => b.PlaceTypeId);

            // Supervisor
            builder.HasOne(b => b.Supervisor)
                .WithOne(b => b.Place)
                .HasForeignKey<Place>(b => b.SupervisorId);

            // Images
            builder.HasMany(b => b.Images)
                .WithOne(b => b.Place)
                .HasForeignKey(b => b.PlaceId);

            // Sports
            builder.HasMany(b => b.Sports)
                .WithOne(b => b.Place)
                .HasForeignKey(b => b.PlaceId);

            // With self
            builder.HasMany(b => b.SubPlaces)
                .WithOne(b => b.ParentPlace)
                .HasForeignKey(b => b.ParentPlaceId);
            builder.Property(b => b.ParentPlaceId).IsRequired(false);
        }
    }
}
