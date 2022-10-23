using Competitions.Domain.Entities.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Competitions.Persistence.Authentication.Mappings
{
    public class RoleMappings : IEntityTypeConfiguration<Role>
    {
        public void Configure ( EntityTypeBuilder<Role> builder )
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedNever();

            builder.Property(b => b.Title).IsUnicode(false);
            builder.Property(b => b.Display);
            builder.Property(b => b.Description).IsRequired(false);
        }
    }
}
