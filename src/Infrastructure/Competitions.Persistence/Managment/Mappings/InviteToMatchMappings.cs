using Competitions.Domain.Entities.Managment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Competitions.Persistence.Managment.Mappings
{
    public class InviteToMatchMappings : IEntityTypeConfiguration<InviteToMatch>
    {
        public void Configure ( EntityTypeBuilder<InviteToMatch> builder )
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedNever();

            builder.Property(b => b.SendTime);
            builder.Property(b => b.IsRead);

            builder.HasOne(b => b.Inviter)
                .WithMany(b => b.Inviters)
                .HasForeignKey(b => b.InviterId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(b => b.Invited)
                .WithMany(b => b.Inviteds)
                .HasForeignKey(b => b.InvitedId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(b => b.Match)
                .WithMany(b => b.Invites)
                .HasForeignKey(b => b.MatchId);

            builder.HasOne(b => b.InviteResult)
                .WithOne(b => b.Invite)
                .HasForeignKey<InviteResult>(b => b.InviteId);
        }
    }
}
