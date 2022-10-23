using Competitions.Domain.Entities.Managment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Competitions.Persistence.Managment.Mappings
{
    public class MatchConditionsMappings : IEntityTypeConfiguration<MatchConditions>
    {
        public void Configure ( EntityTypeBuilder<MatchConditions> builder )
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedNever();

            builder.Property(b => b.Free);
            builder.Property(b => b.Payment);

            builder.OwnsOne(b => b.Regulations , b =>
            {
                b.Property(p => p.Name).HasColumnName("Regulations");
            });
        }
    }
}
