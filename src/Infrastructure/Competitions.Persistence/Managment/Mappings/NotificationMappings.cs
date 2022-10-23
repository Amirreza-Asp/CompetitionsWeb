using Competitions.Domain.Entities.Managment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Competitions.Persistence.Managment.Mappings
{
    public class NotificationMappings : IEntityTypeConfiguration<Notification>
    {
        public void Configure ( EntityTypeBuilder<Notification> builder )
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title);
            builder.Property(b => b.Description);
            builder.Property(b => b.CreateDate);

            builder.OwnsOne(b => b.Image , b =>
            {
                b.Property(p => p.Name).HasColumnName("Image").IsUnicode(false).HasMaxLength(50);
            });
        }
    }
}
