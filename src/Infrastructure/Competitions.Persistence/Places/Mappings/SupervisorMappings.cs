using Competitions.Domain.Entities.Places;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Competitions.Persistence.Places.Mappings
{
    public class SupervisorMappings : IEntityTypeConfiguration<Supervisor>
    {
        public void Configure ( EntityTypeBuilder<Supervisor> builder )
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name);
            builder.OwnsOne(b => b.PhoneNumber , b =>
            {
                b.Property(p => p.Value).HasColumnName("PhoneNumber").IsUnicode(false).HasMaxLength(11);
            });
        }
    }
}
