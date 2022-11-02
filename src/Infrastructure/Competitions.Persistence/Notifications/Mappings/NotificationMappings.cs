using Competitions.Domain.Entities.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Competitions.Persistence.Notifications.Mappings
{
    public class NotificationMappings : IEntityTypeConfiguration<Notification>
    {
        public void Configure ( EntityTypeBuilder<Notification> builder )
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title);
            builder.Property(b => b.Description);
            builder.Property(b => b.CreateDate);

        }
    }
}
