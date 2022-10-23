using Competitions.Domain.Entities.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Competitions.Persistence.Authentication.Mappings
{
    public class UserMappings : IEntityTypeConfiguration<User>
    {
        public void Configure ( EntityTypeBuilder<User> builder )
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedNever();

            builder.Property(b => b.Name);
            builder.Property(b => b.Family);
            builder.Property(b => b.UserName);
            builder.Property(b => b.Password);
            builder.Property(b => b.IsDeleted);
            builder.Property(b => b.RoleId).IsRequired();
            builder.Property(b => b.Gender);


            builder.HasQueryFilter(b => b.IsDeleted == false);

            builder.OwnsOne(b => b.StudentNumber , b =>
            {
                b.Property(p => p.Value).HasColumnName("StudentNumber").IsUnicode(false).HasMaxLength(10);
            });

            builder.OwnsOne(b => b.NationalCode , b =>
            {
                b.Property(p => p.Value).HasColumnName("NationalCode").IsUnicode(false).HasMaxLength(10);
            });

            builder.OwnsOne(b => b.PhoneNumber , b =>
            {
                b.Property(p => p.Value).HasColumnName("PhoneNumber").IsUnicode(false).HasMaxLength(11);
            });


            builder.HasOne(b => b.Role)
                .WithMany(b => b.Users)
                .HasForeignKey(b => b.RoleId);
        }
    }
}
