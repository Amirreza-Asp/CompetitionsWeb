using Competitions.Domain.Entities.Managment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Competitions.Persistence.Managment.Mappings
{
    public class UserTeamMappings : IEntityTypeConfiguration<UserTeam>
    {
        public void Configure ( EntityTypeBuilder<UserTeam> builder )
        {
            builder.Property(b => b.Id).ValueGeneratedNever();
            builder.HasKey(b => b.Id);

            builder.HasOne(b => b.Team)
                .WithMany(b => b.Users)
                .HasForeignKey(b => b.TeamId);

            builder.HasOne(b => b.User)
                .WithMany(b => b.Teams)
                .HasForeignKey(b => b.UserId);
        }
    }
}
