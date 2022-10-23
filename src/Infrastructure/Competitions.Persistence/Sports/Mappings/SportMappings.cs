using Competitions.Domain.Entities.Sports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Competitions.Persistence.Sports.Mappings
{
    public class SportMappings : IEntityTypeConfiguration<Sport>
    {
        public void Configure(EntityTypeBuilder<Sport> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Name);
            builder.Property(b => b.Description).IsRequired(false);
            builder.Property(b => b.CreateDate).HasDefaultValueSql("GETDATE()");

            builder.OwnsOne(b => b.Image, b =>
            {
                b.Property(b => b.Name).HasColumnName("Image").IsRequired();
            });

            builder.HasOne(b => b.SportType)
                .WithMany(b => b.Sports)
                .HasForeignKey(b => b.SportTypeId);
        }
    }
}
