using Competitions.Domain.Entities.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Competitions.Persistence.Notifications.Mappings
{
    public class NotificationImageMappings : IEntityTypeConfiguration<NotificationImage>
    {
        public void Configure ( EntityTypeBuilder<NotificationImage> builder )
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name);
            builder.Property(b => b.SendDate);

            builder.OwnsOne(b => b.Image , b =>
            {
                b.Property(u => u.Name).HasColumnName("Image").IsUnicode(false).HasMaxLength(50);
            });

            builder.HasOne(b => b.Notification)
                .WithMany(b => b.Images)
                .HasForeignKey(b => b.NotificationId);
        }
    }
}
