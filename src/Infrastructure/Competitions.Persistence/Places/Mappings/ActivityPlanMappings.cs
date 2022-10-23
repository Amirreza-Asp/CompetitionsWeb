using Competitions.Domain.Entities.Places;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Competitions.Persistence.Places.Mappings
{
    public class ActivityPlanMappings : IEntityTypeConfiguration<ActivityPlan>
    {
        public void Configure ( EntityTypeBuilder<ActivityPlan> builder )
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name);
            builder.OwnsOne(b => b.File , b =>
            {
                b.Property(p => p.Name).HasColumnName("File").IsUnicode(false).HasMaxLength(50);
            });

            builder.HasOne(b => b.Place)
                .WithOne(b => b.ActivityPlan)
                .HasForeignKey<ActivityPlan>(b => b.PlaceId);
        }
    }
}
