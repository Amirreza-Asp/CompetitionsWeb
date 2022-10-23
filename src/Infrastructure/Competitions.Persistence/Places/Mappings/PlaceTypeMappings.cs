using Competitions.Domain.Entities.Places;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Competitions.Persistence.Places.Mappings
{
    public class PlaceTypeMappings : IEntityTypeConfiguration<PlaceType>
    {
        public void Configure(EntityTypeBuilder<PlaceType> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Title);
            builder.Property(u => u.Description).IsRequired(false);
        }
    }
}
