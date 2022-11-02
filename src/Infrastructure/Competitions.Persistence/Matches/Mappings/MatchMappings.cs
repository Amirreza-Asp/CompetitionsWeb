using Competitions.Domain.Entities.Managment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Competitions.Persistence.Managment.Mappings
{
    public class MatchMappings : IEntityTypeConfiguration<Match>
    {
        public void Configure ( EntityTypeBuilder<Match> builder )
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedNever();

            builder.Property(b => b.Name);
            builder.Property(b => b.Gender);
            builder.Property(b => b.Level);
            builder.Property(b => b.Capacity);
            builder.Property(b => b.TeamCount);
            builder.Property(b => b.CreateDate);
            builder.Property(b => b.RegistrationsNumber).HasDefaultValue(0);
            builder.Property(b => b.Description).IsRequired(false);


            builder.OwnsOne(b => b.Register , b =>
            {
                b.Property(p => p.From).HasColumnName("StartRegsiter");
                b.Property(p => p.To).HasColumnName("EndRegister");
            });

            builder.OwnsOne(b => b.PutOn , b =>
            {
                b.Property(p => p.From).HasColumnName("StartPutOn");
                b.Property(p => p.To).HasColumnName("EndPutOn");
            });

            builder.OwnsOne(b => b.Image , b =>
            {
                b.Property(p => p.Name).HasColumnName("Image").IsUnicode(false).HasMaxLength(50);
            });

            builder.HasOne(b => b.Sport)
                .WithMany(b => b.Matchs)
                .HasForeignKey(b => b.SportId);

            builder.HasOne(b => b.Place)
                .WithMany(b => b.Matchs)
                .HasForeignKey(b => b.PlaceId);

            builder.HasOne(b => b.Festival)
                .WithMany(b => b.Matchs)
                .HasForeignKey(b => b.FestivalId);
            builder.Property(b => b.FestivalId).IsRequired(false);
        }
    }
}
