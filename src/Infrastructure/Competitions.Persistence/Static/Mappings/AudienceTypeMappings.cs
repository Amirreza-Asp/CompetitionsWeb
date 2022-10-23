using Competitions.Domain.Entities.Static;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Competitions.Persistence.Static.Mappings
{
    public class AudienceTypeMappings : IEntityTypeConfiguration<AudienceType>
    {
        public void Configure(EntityTypeBuilder<AudienceType> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Title);
            builder.Property(u => u.Description).IsRequired(false);
        }
    }
}
