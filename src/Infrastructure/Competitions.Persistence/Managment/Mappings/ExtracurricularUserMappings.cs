using Competitions.Domain.Entities.Managment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Competitions.Persistence.Managment.Mappings
{
    public class ExtracurricularUserMappings : IEntityTypeConfiguration<ExtracurricularUser>
    {
        public void Configure ( EntityTypeBuilder<ExtracurricularUser> builder )
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.JoinTime);
            builder.Property(b => b.IsPay);

            builder.HasOne(b => b.User)
                .WithMany(b => b.Extracurriculars)
                .HasForeignKey(b => b.UserId);

            builder.HasOne(b => b.Extracurricular)
                .WithMany(b => b.Users)
                .HasForeignKey(b => b.ExtracurricularId);
        }
    }
}
